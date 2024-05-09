"use strict";

const shippingAccordion = document.querySelector(".shipping-return .top");
const shippingAccordionText = document.querySelector(
  ".shipping-return .bottom"
);
const variationName = document.querySelector(".variation-name span");
const variants = document.querySelectorAll(".variations label");

const slideCount = document.querySelectorAll(
  ".slider .img-container img"
).length;
const imgContainer = document.querySelector(".slider .img-container");
const backBtn = document.querySelector(".slider .slider-back");
const frwrdBtn = document.querySelector(".slider .slider-frwd");
const mobileBackBtn = document.querySelector(".mobile-slider .mobile-back");
const mobileFrwrdBtn = document.querySelector(".mobile-slider .mobile-frwd");
const mobileSlideCount = document.querySelector(".mobile-slider .slide-count");
const slides = document.querySelectorAll(
  ".product-detail-section .left-side .slider .img-container img"
);

let sliderIndex = 0;

// Toggle accordion
shippingAccordion.addEventListener("click", () => {
  shippingAccordionText.classList.toggle("hidden");
  setTimeout(() => {
    shippingAccordionText.classList.toggle("open");
  }, 1);

  // Calculate hegith change due to accordion display property change. For backToTopBtn
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

// Change variations
variationName.textContent = variants[0].textContent;

variants.forEach((variant) => {
  variant.addEventListener("click", () => {
    if (variant.classList.contains("out")) return;

    variants.forEach((label) => {
      label.classList.remove("active");
    });

    variationName.textContent = variant.textContent;
    variant.classList.add("active");
  });
});

const moveSlide = (event) => {
  if (slideCount === 1) return;

  const button = event.target.closest(
    ".slider-back, .slider-frwd, .mobile-back, .mobile-frwd"
  );

  if (!button) return;

  if (button === backBtn || button === mobileBackBtn) {
    if (sliderIndex > 0) sliderIndex--;
    else if (button === backBtn) sliderIndex = slideCount - 4;
    else if (button === mobileBackBtn) sliderIndex = slideCount - 2;
  } else if (button === frwrdBtn || button === mobileFrwrdBtn) {
    if (sliderIndex < slideCount - 4 && button === frwrdBtn) sliderIndex++;
    else if (sliderIndex < slideCount - 2 && button === mobileFrwrdBtn)
      sliderIndex++;
    else sliderIndex = 0;
  }

  let slideWidth = 0;

  slides.forEach((slide) => {
    if (!slide.classList.contains("active")) slideWidth = slide.clientWidth;
  });

  mobileSlideCount.textContent = `${sliderIndex + 1} / ${slideCount - 1}`;

  imgContainer.style.transform = `translateX(${-(
    slideWidth * sliderIndex +
    12 * sliderIndex
  )}px)`;
};

mobileSlideCount.textContent = `1 / ${slideCount - 1}`;

backBtn.addEventListener("click", (event) => moveSlide(event));
frwrdBtn.addEventListener("click", (event) => moveSlide(event));
mobileBackBtn.addEventListener("click", (event) => moveSlide(event));
mobileFrwrdBtn.addEventListener("click", (event) => moveSlide(event));

const checkSlideCount = () => {
  if (slideCount <= 4) {
    backBtn.classList.add("hidden");
    frwrdBtn.classList.add("hidden");
  }
  if (slideCount <= 2) {
    mobileBackBtn.classList.add("hidden");
    mobileFrwrdBtn.classList.add("hidden");
  }
};

if (windowWidth > 425) {
  // Restart main image animation on src change
  const mainImg = document.querySelector(
      ".product-detail-section .left-side .main-img"
    ),
    observer2 = new MutationObserver((changes) => {
      changes.forEach((change) => {
        if (change.attributeName.includes("src")) restartImgAnimation();
      });
    });

  observer2.observe(mainImg, { attributes: true });

  const restartImgAnimation = () => {
    mainImg.classList.remove("run-animation");
    setTimeout(() => mainImg.classList.add("run-animation"), 10);
  };

  // Change main image src on slide click and add "active" to slide
  slides.forEach((slide) => {
    slide.addEventListener("click", () => {
      slides.forEach((img) => {
        img.classList.remove("active");
      });

      slide.classList.add("active");
      if (mainImg.src !== slide.src) mainImg.src = slide.src;
    });
  });
}

checkSlideCount();
