using FMODUnity;
using System.Runtime.ExceptionServices;

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
            [Header("A jouer quand un objet electrique explose (PC/Frigo/Compteur Electrique)")]
            public EventReference electricStart;

            [Header("A jouer quand un objet est chargé electriquement")]
            public EventReference explosionElectric;
        }
        
        [Serializable]
        public class FireSfx
        {
            [Header("Son a jouer lorsque la voiture Explose")]
            [Tooltip("Playoneshot sur la Voiture")]
            public EventReference bigExplosion;

            [Header("Son à jouer en continu lorsqu'un élément est en feu")]
            [Tooltip("Event Emitter Continu qui s'active lorsque l'objet est en feu et se desactive quand il est détruit / s'éteint")]
            public EventReference constantFire;

            [Header ("Son à jouer lors d'une plus petite explosion comme : Le Four / Le Jerrican / Le Radiateur")]
            [Tooltip ("Playoneshot à l'épicentre de l'explosion")]
            public EventReference smallExplosion;

            [Header("Son à jouer lorsqu'un objet prend feu")]
            [Tooltip("Playoneshot sur l'objet qui prend feu. Si possible : lorsque plusieurs objets prennent feu jouer une seule fois ce son mais en volume plus puissant")]
            public EventReference takingFire;
        }

        [Serializable]
        public class WaterSfx
        {
            [Header("Son à jouer quand le joueur détruit : Lavabo, Baignoire, Toilettes, Douche")]
            [Tooltip("Playoneshot sur l'epicentre de l'explosion")]
            public EventReference explosionWater;

            [Header("Pas prio je dois le remix")]
            [Tooltip("")]
            public EventReference waterDrip;

            [Header("A jouer après l'explosion Water")]
            [Tooltip("Event Emitter continu qui commence après l'explosion et se termine quand l'objet est réparé")]
            public EventReference waterStream;

            [Header("Quand un objet est rendu mouillé")]
            [Tooltip("Playone shot sur l'objet")]
            public EventReference WettingWater;
        }
    }
    
    [Serializable]
    public class ObjectsSfx
    {
        [Header("Quand l'objet X touche ou est pick up si spécificité je le précise sinon c'est dans le titre")]
        [Header("Le Reveil/LaRadio/Bougie sont encore à faires je les avais pas vu lors de la collecte")]

        public BookSfx book;

        [Header("Couteaux / Fourchettes")]
        public CutlerySfx cutlery;

        [Header("Verres / Bouteilles")]
        public GlassSfx glass;
        
        public JerricanSfx jerrican;

        [Header("Seau / Scie / Clé à Molette / Tournevis")]
        public MetalSfx metal;

        [Header("Casseroles / Poeles")]
        public PanSfx pan;

        [Header("PQ/Paper")]
        public PaperSfx paper;
        
        public PhoneSfx phone;

        [Header("Serviettes/Tshirts")]
        public TShirtSfx tshirt;
        
        public ToasterSfx toaster;

        [Header("Règle (Tool_04)")]
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
        [Header("Quand le joueur maintient son lancer")]
        [Tooltip("Playoneshot qui doit se jouer quand la touche est maintenue plus longtemps que la moité du temps necessaire au Lancer ")]
        public EventReference playerChargeThrow;

        [Header("Ckanonmeur")]
        public EventReference playerDeath;

        [Header("Se joue quand le joueur se déplace, je vais randomiser le pitch")]
        public EventReference playerFootStep;

        [Header("Quand le joueur sort d'un possédable en l'explosant ou non")]
        [Tooltip("Il est a revoir")]
        public EventReference playerGetOutObject;

        [Header("Quand le joueur lance l'object")]
        [Tooltip("Playone shot quand l'object sort des mains du joueur")]
        public EventReference playerThrow;
    }

    [Serializable]
    public class PNJSfx
    {
        [Header("Quand le PNJ repare un possédable")]
        [Tooltip("A stopper s'il cesse son action")]
        public EventReference repair;

        public FemaleSfx female;
        
        public MaleSfx male;
        
        [Serializable]
        public class FemaleSfx
        {
            [Header("Quand le PNJ se met à courser le joueur")]
            public EventReference femaleChase;

            [Header("A jouer quand le PNJ se balade toutes les 20 secondes")]
            public EventReference femaleSearch;

            [Header("Quand le joueur entre dans le champ de vision du PNJ")]
            public EventReference femaleSpotPlayer;

            [Tooltip("A jouer quand le joueur quitte le champ de vision du PNJ pendant le <Spot> ou après avoir fini une chase")]
            public EventReference femaleSuspicious;
        }

        [Serializable]
        public class MaleSfx
        {
            [Header("Quand le PNJ se met à courser le joueur")]
            public EventReference maleChase;

            [Header("A jouer quand le PNJ se balade toutes les 20 secondes")]
            public EventReference maleSearch;

            [Header("Quand le joueur entre dans le champ de vision du PNJ")]
            public EventReference maleSpotPlayer;

            [Tooltip("A jouer quand le joueur quitte le champ de vision du PNJ pendant le <Spot> ou après avoir fini une chase")]
            public EventReference maleSuspicious;
        }
    }

    [Serializable]
    public class UISfx
    {
        [Header("Quand le joueur clique sur le Main Menu et le Menu Pause")]
        public EventReference uiClick;

        [Tooltip("Quand le joueur passe du menu de base au pc (la transition entre l'image avec le démon a la chaise et l'écran de PC)")]
        public EventReference uiOpenPc;

        [Tooltip("Quand le joueur appuie sur Tab et regarde ses missions, à aussi jouer quand il ferme le menu")]
        public EventReference uiPaperOpen;
    }
}



