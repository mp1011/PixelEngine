public class AcceleratedMotion
{
    public double Speed { get; set; }
    public double Acceleration { get; set; }

    public double Target { get; set; }

    public void Update()
    {
        if (Speed < Target)
        {
            Speed += Acceleration;
            if(Speed > Target)
                Speed = Target;
        }
        else if(Speed > Target)
        {
             Speed -= Acceleration; ;
            if(Speed < Target)
                Speed = Target;
        }
    }

    public void SetSpeedImmediate(int value)
    {
        Speed = value;
        Target = value;
    }
}