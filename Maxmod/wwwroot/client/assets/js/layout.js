"use strict";

const headerAccountEl = document.querySelector(".account-container .wrapper");
const headerAccountDropdownEl = document.querySelector(".account-dropdown");

const headerCartEl = document.querySelector(".header-cart");
const cartDrawerEl = document.querySelector(".cart-container .cart-drawer");
const cartOverlayEl = document.querySelector(".cart-container .overlay");
const cartDrawerCloseBtn = document.querySelector(
  ".cart-container .drawer-top button"
);

const headerCategoriesEl = document.querySelector(".header-bottom .categories");
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
headerCategoriesEl.addEventListener("click", (e) => {
  e.preventDefault();

  if (!e.target.closest(".nav-dropdown")) {
    headerCategoriesDropdown.classList.toggle("hidden");
    if (windowWidth > 1024)
      headerCategoriesChevron.classList.toggle("inverted");
  }
});

// Open drawer on cart click
headerCartEl.addEventListener("click", () => {
  cartDrawerEl.classList.remove("hidden");
  cartOverlayEl.classList.remove("hidden");
  document.body.style.overflow = "hidden";
});

// Close drawer
[cartDrawerCloseBtn, cartOverlayEl].forEach((element) => {
  element.addEventListener("click", () => {
    cartDrawerEl.classList.add("hidden");
    cartOverlayEl.classList.add("hidden");
    document.body.style.overflow = "";
  });
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

// Function to format number to two decimal points
const formatToTwoDecimalPoints = (numStr) => {
  if (typeof numStr === "string") {
    const num = parseFloat(numStr.replace(/[^\d.-]/g, ""));
    if (!isNaN(num)) {
      return "$" + num.toFixed(2);
    }
  }
  return numStr;
};

// Function to handle mutation changes
const handleMutation = (mutationsList, observer) => {
  mutationsList.forEach((mutation) => {
    if (mutation.type === "characterData" || mutation.type === "childList") {
      const target = mutation.target;
      const formattedContent = formatToTwoDecimalPoints(target.textContent);
      target.textContent = formattedContent;
    }
  });
};

// Target elements to observe change
const headerCartTotalEl = document.querySelector(".header-cart-total");
const cartDrawerSubTotalEl = document.querySelector(
  ".cart-drawer .drawer-bottom .subtotal-container .subtotal"
);
const cartDrawerProductTotalEl = document.querySelector(
  ".drawer-middle .product .product-detail .price"
);

// Options for the observer
const config = { characterData: true, childList: true, subtree: true };

// Create a new observer instance
const observer = new MutationObserver(handleMutation);

observer.observe(headerCartTotalEl, config);
observer.observe(cartDrawerSubTotalEl, config);
observer.observe(cartDrawerProductTotalEl, config);
