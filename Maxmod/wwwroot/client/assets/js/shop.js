"use strict";

const filters = document.querySelector(".shop-section .filters");
const filterSortBtn = document.querySelector(".shop-section .filter-sort");
const shopOverlay = document.querySelector(".shop-section .overlay");
const filterCloseBtn = document.querySelector(
  ".shop-section .filters .close-filter"
);

const filterCategoriesBtn = document.querySelector(
  "main .shop-section .filter-categories-btn"
);
const filterCategoriesBtnChevron = document.querySelector(
  "main .shop-section .filter-categories-btn img"
);
const filterCategoryBtns = document.querySelectorAll(
  "main .shop-section .category-btn"
);
const filterCategoriesUls = document.querySelectorAll(
  "main .shop-section .filter-categories-ul"
);
const filterSubcategoriesUls = document.querySelectorAll(
  "main .shop-section .filter-subcategories-ul"
);

const filterBtns = document.querySelectorAll(
  "main .shop-section .filter-wrapper .filter-btn"
);
const filterUls = document.querySelectorAll(
  "main .shop-section .filter-wrapper .filter-ul"
);

const products = document.querySelectorAll(".product");

const disabledBtns = document.querySelectorAll(".pagination .disabled");

// Toggle categories
filterCategoriesBtn.addEventListener("click", () => {
  toggleOpen(filterCategoriesUls);
  filterCategoriesBtnChevron.classList.toggle("inverted");

  filterSubcategoriesUls.forEach((ul) => {
    ul.classList.add("hidden");
    ul.classList.remove("open");
  });
});

// Toggle subcategories
filterCategoryBtns.forEach((btn, index) => {
  btn.addEventListener("click", () => {
    toggleOpen([filterSubcategoriesUls[index]]);
  });
});

// Toggle filters
filterBtns.forEach((btn, index) => {
  btn.addEventListener("click", () => {
    toggleOpen([filterUls[index]]);
  });
});

// Function to toggle open
const toggleOpen = (elements) => {
  elements.forEach((element) => {
    element.classList.toggle("hidden");
    element.classList.toggle("open");
  });
};

// Change variation
products.forEach((product) => {
  const variants = product.querySelectorAll(".variations label");

  variants.forEach((variant) => {
    variant.addEventListener("click", () => {
      if (variant.classList.contains("out")) return;

      const activeVariants = product.querySelectorAll(
        ".variations label.active"
      );
      activeVariants.forEach((label) => {
        label.classList.remove("active");
      });

      variant.classList.add("active");
    });
  });
});

// Disable pagination prev or next buttons
disabledBtns.forEach((btn) => {
  btn.addEventListener("click", (e) => {
    e.preventDefault();
  });
});

// Open-close filter drawer on button click for responsive
shopOverlay.classList.add("hidden");
if (windowWidth <= 768) {
  filters.classList.add("hidden");
  filterSortBtn.addEventListener("click", () => {
    filters.classList.remove("hidden");
    shopOverlay.classList.remove("hidden");
    document.body.style.overflow = "hidden";
  });

  [shopOverlay, filterCloseBtn].forEach((el) => {
    el.addEventListener("click", () => {
      filters.classList.add("hidden");
      shopOverlay.classList.add("hidden");
      document.body.style.overflow = "";
    });
  });
} else {
  filters.classList.remove("hidden");
  shopOverlay.classList.add("hidden");
}

window.addEventListener("resize", () => {
  if (windowWidth <= 768) {
    filters.classList.add("hidden");
    filterSortBtn.addEventListener("click", () => {
      filters.classList.remove("hidden");
      shopOverlay.classList.remove("hidden");
      document.body.style.overflow = "hidden";
    });

    [shopOverlay, filterCloseBtn].forEach((el) => {
      el.addEventListener("click", () => {
        filters.classList.add("hidden");
        shopOverlay.classList.add("hidden");
        document.body.style.overflow = "";
      });
    });
  } else {
    filters.classList.remove("hidden");
    shopOverlay.classList.add("hidden");
  }
});
