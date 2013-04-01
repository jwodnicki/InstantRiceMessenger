using System.Speech.Synthesis;


namespace InstantRiceMessenger
{
	class Speak
	{
		private SpeechSynthesizer synthesizer;
		public Speak()
		{
			synthesizer = new SpeechSynthesizer();
		}
		public void Say(string msg)
		{
			synthesizer.SpeakAsync(msg);
		}
	}
}
