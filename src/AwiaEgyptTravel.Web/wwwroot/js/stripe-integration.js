// Initialize Stripe
const stripe = Stripe(document.querySelector('meta[name="stripe-key"]').getAttribute('content'));

document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('payment-form');
    
    form.addEventListener('submit', async function (event) {
        event.preventDefault();

        // Disable the submit button to prevent multiple submissions
        const submitButton = form.querySelector('button[type="submit"]');
        submitButton.disabled = true;

        try {
            // Create a token for the card
            const { token, error } = await stripe.createToken('card', {
                number: document.getElementById('CardNumber').value,
                exp_month: document.getElementById('ExpiryMonth').value,
                exp_year: document.getElementById('ExpiryYear').value,
                cvc: document.getElementById('Cvc').value
            });

            if (error) {
                // Handle error
                const errorElement = document.getElementById('card-errors');
                errorElement.textContent = error.message;
                submitButton.disabled = false;
                return;
            }

            // Insert the token ID into the form so it gets submitted to the server
            const hiddenInput = document.createElement('input');
            hiddenInput.setAttribute('type', 'hidden');
            hiddenInput.setAttribute('name', 'stripeToken');
            hiddenInput.setAttribute('value', token.id);
            form.appendChild(hiddenInput);

            // Submit the form
            form.submit();
        } catch (err) {
            console.error('Error:', err);
            const errorElement = document.getElementById('card-errors');
            errorElement.textContent = 'An error occurred while processing your payment. Please try again.';
            submitButton.disabled = false;
        }
    });
});
