namespace MARDEK.UI
{
    public class CharacterBagUI : InventoryUI
    {
        private void OnEnable()
        {
            FetchSelectedCharacterAndUpdateUI();
        }

        public void FetchSelectedCharacterAndUpdateUI()
        {
            var character = CharacterSelectable.currentSelected.Character;
            if(character != null)
                AssignInventoryToUI(character.Inventory);
        }
    }
}