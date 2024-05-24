"use strict";

const headerAccountEl = document.querySelector(".account-container .wrapper");
const headerAccountDropdownEl = document.querySelector(".account-dropdown");


const headerCategoriesEl = document.querySelector(".header-bottom .categories");
const headCategoriesLinkEl = document.querySelector(".header-bottom .category-link ")
const headerCategoriesChevron = document.querySelector(
    ".header-bottom .categories img"
);
const headerCategoriesDropdown = document.querySelector(
    ".header-bottom .categories .nav-dropdown"
);

const burgerMenuEl = document.querySelector(".burger-menu");
const headerBottomEl = document.querySelector(".header-bottom");

const backToTopBtn = document.querySelector("footer .back-to-top");

let windowWidth = window.innerWidth;

//Captures window width on window resize
window.addEventListener("resize", () => {
    windowWidth = window.innerWidth;
});

//Toggles header navigation
burgerMenuEl.addEventListener("click", () => {
    burgerMenuEl.classList.toggle("open");
    if (headerBottomEl.style.display == "")
        headerBottomEl.style.display = "block";
    else headerBottomEl.style.display = "";
});

//Toggle header categories dropdown

headCategoriesLinkEl.addEventListener("click", (e) => e.preventDefault())

headerCategoriesEl.addEventListener("click", (e) => {
    if (!e.target.closest(".nav-dropdown")) {
        headerCategoriesDropdown.classList.toggle("hidden");
        if (windowWidth > 1024)
            headerCategoriesChevron.classList.toggle("inverted");
    }
});

// Toggle account dropdown
headerAccountEl.addEventListener("click", (e) => {
    e.preventDefault();
    headerAccountDropdownEl.classList.toggle("hidden");
});

window.addEventListener("click", (e) => {
    const clicked = e.target;
    // Closes header dropdown when clicked on empty space
    if (
        !headerAccountEl.contains(clicked) &&
        !headerAccountDropdownEl.classList.contains("hidden")
    ) {
        headerAccountDropdownEl.classList.add("hidden");
    }

    //Closes header categories dropdown when clicked on empty space
    if (
        !headerCategoriesEl.contains(clicked) &&
        !headerCategoriesDropdown.classList.contains("hidden")
    ) {
        headerCategoriesDropdown.classList.add("hidden");
        headerCategoriesChevron.classList.remove("inverted");
    }

    //Closes header nav when clicked on empty space
    if (!headerBottomEl.contains(clicked) && !clicked.closest(".burger-menu")) {
        burgerMenuEl.classList.remove("open");
        headerBottomEl.style.display = "";
    }
});

// Calculate the percentage of scroll distance
const calculateScrollPercentage = () => {
    const scrollTop = document.documentElement.scrollTop;
    const scrollHeight = document.documentElement.scrollHeight;
    const clientHeight = document.documentElement.clientHeight;

    const scrollDistance = scrollHeight - clientHeight;

    if (scrollDistance === 0) {
        return 0;
    }

    const scrollPercentage = (scrollTop / scrollDistance) * 100;

    return scrollPercentage;
};

// When the user scrolls down 10% from the top of the document, show the button
window.addEventListener("scroll", () => {
    const scrollPercentage = calculateScrollPercentage();

    if (scrollPercentage >= 10) {
        backToTopBtn.style.display = "block";
    } else {
        backToTopBtn.style.display = "none";
    }

    if (scrollPercentage >= 50) {
        backToTopBtn.classList.add("halfway");
    } else {
        backToTopBtn.classList.remove("halfway");
    }

    backToTopBtn.style.background = `linear-gradient(to top, rgb(0, 0, 0) ${Math.round(
        scrollPercentage
    )}%, rgb(255, 255, 255) ${Math.round(scrollPercentage)}%)`;
});

// Scroll to the top of the document
const scrollToTop = () => {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
};

backToTopBtn.addEventListener("click", scrollToTop);


