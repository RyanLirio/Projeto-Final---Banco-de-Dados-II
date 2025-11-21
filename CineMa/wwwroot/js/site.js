document.addEventListener("DOMContentLoaded", () => {
    const moneyFields = document.querySelectorAll(".money");

    moneyFields.forEach(field => {
        const rawValue = parseInt(field.dataset.value); // ex: 2000
        const formatted = (rawValue / 100).toLocaleString("pt-BR", {
            style: "currency",
            currency: "BRL"
        });

        field.textContent = formatted;
    });
});

function formatPrice(input) {
    let value = input.value.replace(/\D/g, '');

    if (value.length > 2) {
        value = value.slice(0, -2) + ',' + value.slice(-2);
    }

    input.value = value;
}