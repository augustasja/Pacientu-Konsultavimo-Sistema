function confirmDelete(uniqId, isDeleteClicked)
{
	var deleteSpan = 'deleteSpan_' + uniqId; 
	var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqId; 

	if (isDeleteClicked)
	{
		$('#' + deleteSpan).hide();
		$('#' + confirmDeleteSpan).show();
	}
	else
	{
		$('#' + deleteSpan).show();
		$('#' + confirmDeleteSpan).hide();
	}
}
