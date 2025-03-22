// Örnek ürün veritabanı
const products = {
    '8690504037554': { name: 'Ülker Çikolata', price: 5.50, image: 'https://via.placeholder.com/150' },
    '8690504037555': { name: 'Eti Burçak', price: 15.90, image: 'https://via.placeholder.com/150' },
    '8690504037556': { name: 'Sütaş Süt', price: 12.50, image: 'https://via.placeholder.com/150' },
    '8690504037557': { name: 'Torku Ekmek', price: 5.00, image: 'https://via.placeholder.com/150' }
};

// Sepet
let cart = {};

// DOM elementleri
const barcodeInput = document.getElementById('barcodeInput');
const productGrid = document.getElementById('productGrid');
const cartItems = document.getElementById('cartItems');
const cartTotal = document.getElementById('cartTotal');
const completePaymentBtn = document.getElementById('completePayment');

// Barkod okuma işlemi
barcodeInput.addEventListener('keypress', (e) => {
    if (e.key === 'Enter') {
        const barcode = barcodeInput.value;
        if (products[barcode]) {
            addToCart(barcode);
            barcodeInput.value = '';
        } else {
            alert('Ürün bulunamadı!');
        }
    }
});

// Sepete ürün ekleme
function addToCart(barcode) {
    if (cart[barcode]) {
        cart[barcode].quantity++;
    } else {
        cart[barcode] = {
            ...products[barcode],
            quantity: 1
        };
    }
    updateCartDisplay();
}

// Sepetten ürün çıkarma
function removeFromCart(barcode) {
    if (cart[barcode].quantity > 1) {
        cart[barcode].quantity--;
    } else {
        delete cart[barcode];
    }
    updateCartDisplay();
}

// Ürünü tamamen sepetten kaldırma
function removeItemCompletely(barcode) {
    delete cart[barcode];
    updateCartDisplay();
}

// Sepet görüntüsünü güncelleme
function updateCartDisplay() {
    cartItems.innerHTML = '';
    let total = 0;

    Object.entries(cart).forEach(([barcode, item]) => {
        const itemTotal = item.price * item.quantity;
        total += itemTotal;

        const cartItem = document.createElement('div');
        cartItem.className = 'cart-item';
        cartItem.innerHTML = `
            <div class="cart-item-info">
                <h5>${item.name}</h5>
                <p>${item.price.toFixed(2)} TL x ${item.quantity}</p>
                <p>Toplam: ${itemTotal.toFixed(2)} TL</p>
            </div>
            <div class="cart-item-quantity">
                <button onclick="removeFromCart('${barcode}')">-</button>
                <span>${item.quantity}</span>
                <button onclick="addToCart('${barcode}')">+</button>
            </div>
            <button class="remove-item" onclick="removeItemCompletely('${barcode}')">
                <i class="fas fa-times"></i>
            </button>
        `;
        cartItems.appendChild(cartItem);
    });

    cartTotal.textContent = `${total.toFixed(2)} TL`;
}

// Ödeme işlemi
completePaymentBtn.addEventListener('click', () => {
    if (Object.keys(cart).length === 0) {
        alert('Sepetiniz boş!');
        return;
    }

    // Burada POS cihazına yönlendirme yapılacak
    alert('POS cihazına yönlendiriliyorsunuz...');
    
    // Örnek olarak sepeti temizleme
    cart = {};
    updateCartDisplay();
});

// Sayfa yüklendiğinde ürünleri göster
function displayProducts() {
    Object.entries(products).forEach(([barcode, product]) => {
        const productCard = document.createElement('div');
        productCard.className = 'product-card';
        productCard.innerHTML = `
            <img src="${product.image}" alt="${product.name}">
            <h5>${product.name}</h5>
            <p>${product.price.toFixed(2)} TL</p>
            <button class="btn btn-primary" onclick="addToCart('${barcode}')">
                Sepete Ekle
            </button>
        `;
        productGrid.appendChild(productCard);
    });
}

// Sayfa yüklendiğinde ürünleri göster
displayProducts(); 