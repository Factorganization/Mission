using Runtime.Services.Game.GameContent.Logics.LogicControllers.ElementControllers;

namespace Runtime.Services.Game.GameContent.Logics.LogicModels.ElementModels;

public record struct ElementInteractionDataPair(int Flag, ElementInteraction Callback);