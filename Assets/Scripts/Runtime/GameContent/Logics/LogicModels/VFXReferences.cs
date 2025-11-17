using UnityEngine;

namespace Runtime.GameContent.Logics.LogicModels
{
	[System.Serializable]
	public class VFXReferences
	{
		public ParticleSystem fireParticles;

		public ParticleSystem waterParticles;

		public ParticleSystem electricParticles;

		public ParticleSystem explosionParticles;

		public bool firePlaying;
		
		public bool waterPlaying;
		
		public bool elecPlaying;
		
		public bool explodePlaying;
	}
}
