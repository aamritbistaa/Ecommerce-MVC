
const baseUrl = "https://fakestoreapi.com/products";
const newProduct = document.getElementById("New-Products");
const searchBar = document.getElementById("Search-Bar");
const btnContainer = document.querySelector("#btn-container");

let products = [];
let index = 0;
let pages = [];

const setupUI = () => {
    displayProducts(pages[index]);
    displayButtons(btnContainer, pages, index);
}

const init = async () => {
    await loadProducts();
    pages = paginate(products);
    setupUI();
}

btnContainer.addEventListener('click', function (e) {
    if (e.target.classList.contains('btn-container')) return;

    if (e.target.classList.contains('page-btn')) {
        index = parseInt(e.target.dataset.index);
    }

    if (e.target.classList.contains('next-btn')) {
        index++;
        if (index > pages.length - 1) {
            index = 0;
        }
    }

    if (e.target.classList.contains('prev-btn')) {
        index--;
        if (index < 0) {
            index = pages.length - 1;
        }
    }
    setupUI();

});

//searchBar
searchBar.addEventListener("keyup", function (e) {
    const searchString = e.target.value.toLowerCase();
    const filterProducts = products.filter((item) => {
        return item.category.toLowerCase().includes(searchString) || item.title.toLowerCase().includes(searchString);
    });
    displayProducts(filterProducts)
})

//load products
const loadProducts = async () => {
    try {
        const res = await fetch(baseUrl);
        console.log(baseUrl);
        products = await res.json();
        displayProducts(products);
        console.log(products);
        if (!res.ok) throw new Error(`${res.statusText} (${res.status})`);
    }
    catch (err) {
        console.log(err)
    }
}
loadProducts();

const displayProducts = (products) => {
    const htmlString = products.map((item) => {
        return `
                        <div class ="col-md-4">
                            <div class="card">
                                <img class="card-img-top" width="40" src="${item.image}" alt="img"/>
                                <div class="card-body">
                                <h2 class="card-title">${item.title}</h2>
                                    <h5>${item.category}</h5>
                                    <a href="#" class="btn btn-primary">Go to products</a>
                                </div>
                            </div>
                        </div>
                        `
    }).join('');
    newProduct.innerHTML = htmlString;
}

const paginate = (items) => {
    const itemsPerPage = 9;
    const numberOfPages = Math.ceil(items.length / itemsPerPage);
    const newPages = Array.from({ length: numberOfPages }, (_, index) => {
        const start = index * itemsPerPage;
        return items.slice(start, start + itemsPerPage);
    })
    return newPages;
}

const displayButtons = (btnContainer, pages, activeIndex) => {
    let btns = pages.map((_, pageIndex) => {
        return `<button class="page-btn ${activeIndex === pageIndex ? 'active-btn' : ''}" data-index="${pageIndex}">
                        ${pageIndex + 1}</button>`;
    });
    btns.push(`<button class="prev-btn">prev</button>`);
    btnContainer.innerHTML = btns.join('');
}
displayButtons(btnContainer, pages, index);
window.addEventListener('load', init);
