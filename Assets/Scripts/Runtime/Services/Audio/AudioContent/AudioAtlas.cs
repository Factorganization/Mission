using FMODUnity;

namespace Runtime.Services.Audio.AudioContent;

[Serializable]
public class AudioAtlas
{
    public Musics musics;
    
    public SFX sfx;
}

[Serializable]
public class Musics
{
    
}

[Serializable]
public class SFX
{
    public EffectsSfx effects;
    
    public ObjectsSfx objects;

    public PlayerSfx player;

    public PNJSfx pnj;

    public UISfx ui;
    
    [Serializable]
    public class EffectsSfx
    {
        public ElectricitySfx electricity;
        
        public FireSfx fire;
        
        public WaterSfx water;
        
        [Serializable]
        public class ElectricitySfx
        {
            public EventReference electricStart;

            public EventReference explosionElectric;
        }
        
        [Serializable]
        public class FireSfx
        {
            public EventReference bigExplosion;

            public EventReference constantFire;

            public EventReference smallExplosion;

            public EventReference takingFire;
        }

        [Serializable]
        public class WaterSfx
        {
            public EventReference explosionWater;

            public EventReference waterDrip;

            public EventReference waterStream;

            public EventReference WettingWater;
        }
    }
    
    [Serializable]
    public class ObjectsSfx
    {
        public BookSfx book;
        
        public CutlerySfx cutlery;
        
        public GlassSfx glass;
        
        public JerricanSfx jerrican;
        
        public MetalSfx metal;

        public PanSfx pan;
        
        public PaperSfx paper;
        
        public PhoneSfx phone;
        
        public TShirtSfx tshirt;
        
        public ToasterSfx toaster;
        
        public WoodSfx wood;
        
        [Serializable]
        public class BookSfx
        {
            public EventReference bookHit;
            
            public EventReference bookPick;
        }

        [Serializable]
        public class CutlerySfx
        {
            public EventReference cutleryHit;
            
            public EventReference cutleryPick;
        }

        [Serializable]
        public class GlassSfx
        {
            public EventReference glassHit;
            
            public EventReference glassPick;
        }

        [Serializable]
        public class JerricanSfx
        {
            public EventReference jerricanHit;
            
            public EventReference jerricanPick;
        }

        [Serializable]
        public class MetalSfx
        {
            public EventReference metalHit;
            
            public EventReference metalPick;
        }

        [Serializable]
        public class PanSfx
        {
            public EventReference panHit;
            
            public EventReference panPick;
        }

        [Serializable]
        public class PaperSfx
        {
            public EventReference paperHit;
            
            public EventReference paperPick;
        }

        [Serializable]
        public class PhoneSfx
        {
            public EventReference phoneHit;
            
            public EventReference phonePick;
        }

        [Serializable]
        public class TShirtSfx
        {
            public EventReference tshirtHit;
            
            public EventReference tshirtPick;
        }

        [Serializable]
        public class ToasterSfx
        {
            public  EventReference toasterHit;
            
            public EventReference toasterPick;
        }

        [Serializable]
        public class WoodSfx
        {
            public EventReference woodHit;
            
            public EventReference woodPick;
        }
    }
    
    [Serializable]
    public class PlayerSfx
    {
        public EventReference playerChargeThrow;
        
        public EventReference playerDeath;

        public EventReference playerFootStep;

        public EventReference playerGetOutObject;

        public EventReference playerThrow;
    }

    [Serializable]
    public class PNJSfx
    {
        public EventReference repair;

        public FemaleSfx female;
        
        public MaleSfx male;
        
        [Serializable]
        public class FemaleSfx
        {
            public EventReference femaleChase;

            public EventReference femaleSearch;
            
            public EventReference femaleSpotPlayer;

            public EventReference femaleSuspicious;
        }

        [Serializable]
        public class MaleSfx
        {
            public EventReference maleChase;
            
            public EventReference maleSearch;
            
            public EventReference maleSpotPlayer;
            
            public EventReference maleSuspicious;
        }
    }

    [Serializable]
    public class UISfx
    {
        public EventReference uiClick;

        public EventReference uiOpenPc;

        public EventReference uiPaperOpen;
    }
}



