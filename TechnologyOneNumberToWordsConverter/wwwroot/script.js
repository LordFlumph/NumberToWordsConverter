const form = document.getElementById('convertForm');

form.addEventListener('submit', function (event) {
	event.preventDefault();

	const input = document.getElementById('amount');
	const resultText = document.getElementById('result');
	const numberAmount = input.value;
	console.log(numberAmount);
	fetch('/api/convert',
	{
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({ amount: numberAmount })
		})
		.then(response => response.text())
		.then(words => { resultText.textContent = words; })
		.catch(error =>
		{
			console.error('Error:', error);
			resultText.style.color = 'red';
			resultText.textContent = error;
		});
});