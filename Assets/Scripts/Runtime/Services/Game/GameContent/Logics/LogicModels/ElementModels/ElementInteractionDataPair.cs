using Runtime.Services.GameService.GameContent.Logics.LogicControllers.ElementControllers;

namespace Runtime.Services.GameService.GameContent.Logics.LogicModels.ElementModels;

public record struct ElementInteractionDataPair(int Flag, ElementInteraction Callback);