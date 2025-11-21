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
