public class PlayerController
{     
    private readonly CollisionManager _collisionManager;
    private readonly InputManager _inputManager;
    private readonly KeyedTileAnimation<PlayerAnimations> _playerAnimations;
    private readonly MovingSprite _player;
    private readonly SpriteCollider _collider;

    private bool _inSlide;
    private int _highJumpCounter;

    public PlayerController(InputManager inputManager, KeyedTileAnimation<PlayerAnimations> playerAnimations, MovingSprite player,
        CollisionManager collisionManager)
    {
        _inputManager = inputManager;
        _collisionManager = collisionManager;
        _playerAnimations = playerAnimations;
        _player = player;

        _player.HorizontalMotion.Target = 0;
        _player.HorizontalMotion.Acceleration = MotionConstants.PlayerAccel;
        _player.VerticalMotion.Acceleration = MotionConstants.PlayerGravity;


        _collider = new SpriteCollider(
            HorizontalCollider: new Rectangle(0, 4, _player.PixelWidth, _player.PixelHeight - 8),
            VerticalCollider: new Rectangle(4, 0, _player.PixelWidth - 8, _player.PixelHeight));
    }

    public double PlayerMoveSpeed => _inputManager.Player1.KeyDown(GamepadButtons.A) ? MotionConstants.PlayerRunSpeed : MotionConstants.PlayerWalkSpeed;

   
    public void Update()
    {
        var isOnGround = _collisionManager.CheckBlockCollision(_player, _collider).IsOnGround;

        if (_player.HorizontalMotion.Speed == 0)
            _playerAnimations.DurationScale = 1.0;
        else
        {
            var percent = Math.Abs(_player.HorizontalMotion.Speed / MotionConstants.PlayerWalkSpeed);
            _playerAnimations.DurationScale = (1.0 / percent).Clamp(0.5, 2.0);
            
        }

        if (_inputManager.Player1.KeyDown(GamepadButtons.Left))
        {
            _player.HorizontalMotion.Target = -PlayerMoveSpeed;
            _playerAnimations.CurrentAnimation = PlayerAnimations.Walk;
        }
        else if (_inputManager.Player1.KeyDown(GamepadButtons.Right))
        {
            _player.HorizontalMotion.Target = PlayerMoveSpeed;
            _playerAnimations.CurrentAnimation = PlayerAnimations.Walk;
        }
        else
        {
            _player.HorizontalMotion.Target = 0;
            _playerAnimations.CurrentAnimation = PlayerAnimations.Idle;
        }

        if(!_inSlide)
        {
            if(!isOnGround)
            {
                if (_player.HorizontalMotion.Speed != 0)
                {
                    _player.Sprite.HorizontalFlip = _player.HorizontalMotion.Speed < 0;
                }
            }
            else if (_player.HorizontalMotion.Target < 0 && _player.HorizontalMotion.Speed > 0)
            {
                _player.Sprite.HorizontalFlip = true;
                _inSlide = true;
            }
            else if (_player.HorizontalMotion.Target > 0 && _player.HorizontalMotion.Speed < 0)
            {
                _player.Sprite.HorizontalFlip = false;
                _inSlide = true;
            }
            else if(_player.HorizontalMotion.Speed != 0)
            {
                _player.Sprite.HorizontalFlip = _player.HorizontalMotion.Speed < 0;
            }
        }
        else
        {
            if (_player.HorizontalMotion.Target < 0 && _player.HorizontalMotion.Speed < -MotionConstants.PlayerSlideLimit)
                _inSlide = false;
            else if (_player.HorizontalMotion.Target > 0 && _player.HorizontalMotion.Speed > MotionConstants.PlayerSlideLimit)
                _inSlide = false;
            else if (_player.HorizontalMotion.Target == 0)
                _inSlide = false;
            else
                _playerAnimations.CurrentAnimation = PlayerAnimations.Slide;
        }

        if(isOnGround && _inputManager.Player1.KeyPressed(GamepadButtons.B))
        {            
            _player.VerticalMotion.Speed = MotionConstants.PlayerJumpSpeed;
            _highJumpCounter = 0;
        }


        if (!isOnGround)
        {
            _player.VerticalMotion.Target = MotionConstants.PlayerFallSpeed;
            if (_inputManager.Player1.KeyDown(GamepadButtons.B) && _highJumpCounter < MotionConstants.HighJumpFrames)
            {
                _highJumpCounter++;
                _player.VerticalMotion.Speed = MotionConstants.PlayerJumpSpeed;
            }

            _inSlide = false;
            if (_player.VerticalMotion.Speed < 0)
                _playerAnimations.CurrentAnimation = PlayerAnimations.Jump;
            else
                _playerAnimations.CurrentAnimation = PlayerAnimations.Fall;

            if (_player.HorizontalMotion.Speed != 0)
                _player.Sprite.HorizontalFlip = _player.HorizontalMotion.Speed < 0;
        }

        _player.Update();
    }
} 
