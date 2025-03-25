let cart = {};
let tempBarcode = null; // Geçici barkod değişkeni


// Ödeme işlemi
function completepay() {
    if (Object.keys(cart).length === 0) {
        alert('Sepetiniz boş!');
        return;
    }

    alert('POS cihazına yönlendiriliyorsunuz...');
    cart = {}; // Sepeti temizle
    updateCartDisplay();
}



function handleEnterKey() {
    const barcodeInput = document.getElementById('barcodeInput');


    const barcode = barcodeInput.value;
    
    if (barcodeInput.value.length == 13) {
        addToCart(barcode);
        barcodeInput.value = '';
    }
}




function addToCart(barcode) {
    // Ürünün HTML'deki ilgili elemanını bul
    const productElement = document.querySelector(`.product-card[data-id="${barcode}"]`);

    if (!productElement) {
        console.error("Ürün bulunamadı!");
        return;
    }

    // Ürün bilgilerini data-* attribute'larından al
    const name = productElement.getAttribute('data-name');
    const salePrice = parseFloat(productElement.getAttribute('data-price'));
    const image = productElement.getAttribute('data-image');

    if (cart[barcode]) {
        cart[barcode].quantity++;
    } else {
        cart[barcode] = { barcode, name, salePrice, image, quantity: 1 };
    }

    updateCartDisplay();
}


// Update the cart display
function updateCartDisplay() {
    const cartItems = document.getElementById('cartItems');
    const cartTotal = document.getElementById('cartTotal');
    cartItems.innerHTML = ''; // Clear the cart
    let total = 0;

    Object.values(cart).forEach(item => {
        const itemTotal = item.salePrice * item.quantity; // Total price for this item
        total += itemTotal;

        const cartItem = document.createElement('div');
        cartItem.className = 'cart-item';
        cartItem.innerHTML = `
            <div class="cart-item-info">
                <h5>${item.name}</h5>
                <p>${item.salePrice} TL x ${item.quantity}</p>
                <p>Toplam: ${itemTotal} TL</p>
            </div>
            <div class="cart-item-quantity">
                <button onclick="removeFromCart('${item.barcode}')">-</button>
                <span>${item.quantity}</span>
                <button onclick="addProductToCart('${item.barcode}')">+</button>
            </div>
            <button class="remove-item" onclick="removeItemCompletely('${item.barcode}')">
                <i class="fas fa-times"></i>
            </button>
        `;
        cartItems.appendChild(cartItem);
    });

    cartTotal.textContent = `${total} TL`; // Update total cart price
}


// Sepete Ekle fonksiyonunu güncelleme
function addProductToCart(barcode) {
    const barcodeInput = document.getElementById('barcodeInput');
    if (tempBarcode) {
        fetchProduct(tempBarcode);
        tempBarcode = null; // İşlem tamamlandıktan sonra sıfırla
        barcodeInput.value = ''; // Input'u temizle
    } else {
        // Eğer barkod okutulmadıysa, direkt olarak ürün kartındaki barkodu kullan
        fetchProduct(barcode);
    }
}

// Barkoda göre ürünü getir ve sepete ekle
async function fetchProduct(barcode) {
    try {
        const response = await fetch(`/Satis/GetProductByBarcode?barcode=${barcode}`);
        if (!response.ok) {
            throw new Error('Ürün bulunamadı!');
        }

        const product = await response.json(); // Gelen JSON verisini al
        addToCart(product.barcode, product.name, product.salePrice, product.imageUrl);
    } catch (error) {
        alert(error.message);
    }
}



// Sepetten ürün çıkarma
function removeFromCart(barcode) {
    if (cart[barcode].quantity > 1) {
        cart[barcode].quantity--; // Miktar bir azalacak
    } else {
        delete cart[barcode]; // Ürün tamamen silinecek
    }
    updateCartDisplay();
}

// Ürünü tamamen sepetten kaldırma
function removeItemCompletely(barcode) {
    delete cart[barcode];
    updateCartDisplay();
}