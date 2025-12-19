using Runtime.Services.GameService.GameContent.Logics.LogicInterfaces;

namespace Runtime.Services.GameService.GameContent.Logics.LogicModels.ElementModels;

/// <summary>
/// Init element holder pair
/// </summary>
/// <param name="Holder1">IS holding element</param>
/// <param name="Holder2">CAN hold element</param>
public record struct ElementInteractionData(IElementHolder Holder1, IElementHolder Holder2);