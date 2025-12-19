using Runtime.Services.GameService.GameContent.Logics.LogicModels.ElementModels;

namespace Runtime.Services.GameService.GameContent.Logics.LogicControllers.ElementControllers
{
	/// <summary>
	/// simple delegate that pass 2 element holders as parameters
	/// </summary>
	/// <param name="elementInteractionData"></param>
    public delegate void ElementInteraction(ElementInteractionData elementInteractionData);
}