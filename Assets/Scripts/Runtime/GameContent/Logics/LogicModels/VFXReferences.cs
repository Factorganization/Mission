using UnityEngine;

namespace Runtime.GameContent.Logics.LogicModels
{
	[System.Serializable]
	public class VFXReferences
	{
		public ParticleSystem[] fireParticles;

		public ParticleSystem[] waterParticles;

		public ParticleSystem[] electricParticles;

		public ParticleSystem[] explosionParticles;

		[HideInInspector] public bool firePlaying;
		
		[HideInInspector] public bool waterPlaying;
		
		[HideInInspector] public bool elecPlaying;
		
		[HideInInspector] public bool explodePlaying;
	}
}
