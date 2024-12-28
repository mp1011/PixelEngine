public class PlayerController
{     
    private readonly InputManager _inputManager;
    private readonly KeyedTileAnimation<PlayerAnimations> _playerAnimations;
    private readonly MovingSprite _player;
    private bool _inSlide;

    public PlayerController(InputManager inputManager, KeyedTileAnimation<PlayerAnimations> playerAnimations, MovingSprite player)
    {
        _inputManager = inputManager;
        _playerAnimations = playerAnimations;
        _player = player;

        _player.HorizontalMotion.Target = 0;
        _player.HorizontalMotion.Acceleration = MotionConstants.PlayerAccel;
        _player.VerticalMotion.Acceleration = MotionConstants.PlayerGravity;
    }

    public double PlayerMoveSpeed => _inputManager.Player1.KeyDown(GamepadButtons.A) ? MotionConstants.PlayerRunSpeed : MotionConstants.PlayerWalkSpeed;

    public bool IsOnGround => _player.Sprite.VerticalPos == 260;

    public void Update()
    {
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
            if(!IsOnGround)
            {
                _player.Sprite.HorizontalFlip = _player.HorizontalMotion.Speed < 0;
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
            else
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
            else
                _playerAnimations.CurrentAnimation = PlayerAnimations.Slide;
        }

        if(IsOnGround && _inputManager.Player1.KeyDown(GamepadButtons.B))
        {
            _player.VerticalMotion.Target = MotionConstants.PlayerFallSpeed;
            _player.VerticalMotion.Speed = MotionConstants.PlayerJumpSpeed;             
        }

        if(!IsOnGround)
        {
            _inSlide = false;
            if (_player.VerticalMotion.Speed < 0)
                _playerAnimations.CurrentAnimation = PlayerAnimations.Jump;
            else
                _playerAnimations.CurrentAnimation = PlayerAnimations.Fall;
        }

        //temp
        if(_player.Sprite.VerticalPos > 260)
        {
            _player.Sprite.VerticalPos = 260;
            _player.RealY = 260;
            _player.VerticalMotion.Target = 0;
            _player.VerticalMotion.Speed = 0;
        }

        _player.Update();
    }
} 
