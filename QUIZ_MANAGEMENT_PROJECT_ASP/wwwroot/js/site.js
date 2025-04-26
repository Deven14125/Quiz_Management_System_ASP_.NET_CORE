function filterQuizList() {
    // Get filter input values
    const quizNameInput = document.getElementById('QuizName').value.toLowerCase();
    const minQuestionInput = parseInt(document.getElementById('MinQuestion').value);
    const maxQuestionInput = parseInt(document.getElementById('MaxQuestion').value);
    const startDateInput = new Date(document.getElementById('StartDate').value);
    const endDateInput = new Date(document.getElementById('EndDate').value);

    // Get all rows from the quiz table
    const rows = document.querySelectorAll('tbody tr');

    // Loop through the rows and filter them
    rows.forEach(row => {
        const quizName = row.cells[1].textContent.toLowerCase(); // Assuming Quiz Name is in the second column
        const totalQuestions = parseInt(row.cells[2].textContent); // Assuming Total Questions is in the third column
        const quizDate = new Date(row.cells[3].textContent); // Assuming Quiz Date is in the fourth column

        let displayRow = true;

        // Apply Quiz Name filter
        if (quizNameInput && !quizName.includes(quizNameInput)) {
            displayRow = false;
        }

        // Apply Min Question filter
        if (!isNaN(minQuestionInput) && totalQuestions < minQuestionInput) {
            displayRow = false;
        }

        // Apply Max Question filter
        if (!isNaN(maxQuestionInput) && totalQuestions > maxQuestionInput) {
            displayRow = false;
        }

        // Apply Start Date filter
        if (!isNaN(startDateInput.getTime()) && quizDate < startDateInput) {
            displayRow = false;
        }

        // Apply End Date filter
        if (!isNaN(endDateInput.getTime()) && quizDate > endDateInput) {
            displayRow = false;
        }

        // Show or hide the row based on filters
        row.style.display = displayRow ? '' : 'none';
    });
}