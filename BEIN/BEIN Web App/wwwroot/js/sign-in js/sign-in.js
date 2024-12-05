document.addEventListener('DOMContentLoaded', () => {
    const container = document.querySelector('.container');
    const registerBtn = document.querySelector('.register-btn');
    const loginBtn = document.querySelector('.login-btn');

    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        const formBtn = form.querySelector('button');
        formBtn.addEventListener('click', function () {
            if (formBtn.type === "button") {
                if (validateForm(form)) {
                    formBtn.type = "submit";
                    formBtn.click();
                    formBtn.type = "button";
                }
            }
        });
    });

    registerBtn.addEventListener('click', () => {
        container.classList.add('active');
    });

    loginBtn.addEventListener('click', () => {
        container.classList.remove('active');
    });

    function validateForm(form) {
        const inputs = form.querySelectorAll('input');
        let isValid = true;

        inputs.forEach(input => {
            input.classList.remove('error');

            if (input.hasAttribute('required') && input.value.trim() === '') {
                input.classList.add('error');
                isValid = false;
            }

            if (input.type === 'email') {
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailRegex.test(input.value)) {
                    input.classList.add('error');
                    isValid = false;
                }
            }

            if (input.type === 'password' && input.value.length < 6) {
                input.classList.add('error');
                isValid = false;
            }
        });

        return isValid;
    }
});