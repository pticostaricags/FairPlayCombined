function initPayPalButton(dotNetHelper) {
    if (window.paypal === undefined) {
        dotNetHelper.invokeMethodAsync('ShowPayPalNotFoundError');
    }
    else {
        let shipping = 0;
        let itemOptions = document.querySelector("#smart-button-container #item-options");
        let quantity = parseInt();
        let quantitySelect = document.querySelector("#smart-button-container #quantitySelect");
        if (!isNaN(quantity)) {
            quantitySelect.style.visibility = "visible";
        }
        let orderDescription = 'Fund';
        if (orderDescription === '') {
            orderDescription = 'Item';
        }
        paypal.Buttons({
            style: {
                shape: 'rect',
                color: 'blue',
                layout: 'vertical',
                label: 'paypal',

            },
            createOrder: function (data, actions) {
                let selectedItemDescription = itemOptions.options[itemOptions.selectedIndex].value;
                let selectedItemPrice = parseFloat(itemOptions.options[itemOptions.selectedIndex].getAttribute("price"));
                let tax = (selectedItemPrice === 0) ? 0 : (selectedItemPrice * (parseFloat(selectedItemPrice) / 100));
                if (quantitySelect.options.length > 0) {
                    quantity = parseInt(quantitySelect.options[quantitySelect.selectedIndex].value);
                } else {
                    quantity = 1;
                }

                tax *= quantity;
                tax = Math.round(tax * 100) / 100;
                let priceTotal = quantity * selectedItemPrice + parseFloat(shipping) + tax;
                priceTotal = Math.round(priceTotal * 100) / 100;
                let itemTotalValue = Math.round((selectedItemPrice * quantity) * 100) / 100;

                return actions.order.create({
                    purchase_units: [{
                        description: orderDescription,
                        amount: {
                            currency_code: 'USD',
                            value: priceTotal,
                            breakdown: {
                                item_total: {
                                    currency_code: 'USD',
                                    value: itemTotalValue,
                                },
                                shipping: {
                                    currency_code: 'USD',
                                    value: shipping,
                                },
                                tax_total: {
                                    currency_code: 'USD',
                                    value: tax,
                                }
                            }
                        },
                        items: [{
                            name: selectedItemDescription,
                            unit_amount: {
                                currency_code: 'USD',
                                value: selectedItemPrice,
                            },
                            quantity: quantity
                        }]
                    }]
                });
            },
            onApprove: function (data, actions) {
                return actions.order.capture().then(function (details) {
                    dotNetHelper.invokeMethodAsync('OnApprove', data, details);
                });
            },
            onError: function (err) {
                console.log(err);
            },
        }).render('#paypal-button-container');
    }
}