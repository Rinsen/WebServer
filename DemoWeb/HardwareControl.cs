using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
namespace DemoWeb
{
    public static class HardwareControl
    {
        static HardwareControl()
        {
            OnBoardLed = new PinControl(new OutputPort(Pins.ONBOARD_LED, false));
        }

        public static PinControl OnBoardLed { get; private set; }
    }

    public class PinControl
    {
        private readonly OutputPort _ledPort;

        public PinControl(OutputPort ledPort)
        {
            _ledPort = ledPort;
        }

        public void TurnOn()
        {
            _ledPort.Write(true);
        }

        public void TurnOff()
        {
            _ledPort.Write(false);
        }

        public bool GetStatus()
        {
            return _ledPort.Read();
        }
    }
}
