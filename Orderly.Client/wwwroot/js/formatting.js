
/*
    Currency Formatting
*/

function initCurrencyFormatter() {
    // Find all containers with class input-currency

    const currencyInputs = document.querySelectorAll('.input-currency input[type="number"]');

    currencyInputs.forEach(input => {
        // Format current value on load

        formatInput(input);

        // On blur, format to 2 decimal places

        input.addEventListener('blur', () => {
            formatInput(input);
        });

        // Prevent invalid chars, only allow digits and decimal points
        // and at most 1 decimal

        input.addEventListener('input', () => {
            let val = input.value;

            // Remove invalid characters (only digits and 1 decimal allowed)

            val = val.replace(/[^0-9.]/g, '');

            const parts = val.split('.');
            if (parts.length > 2) {
                val = parts.shift() + '.' + parts.join('');
            }

            input.value = val;
        });
    });

    function formatInput(input) {
        if (input.value === '') return;

        const num = parseFloat(input.value);
        if (!isNaN(num)) {
            input.value = num.toFixed(2);
        } else {
            input.value = '';
        }
    }
};
