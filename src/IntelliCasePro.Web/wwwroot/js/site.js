window.addEventListener("DOMContentLoaded", () => {
  const flash = document.getElementById("flashMessage");
  if (flash) {
    setTimeout(() => flash.classList.add("hide"), 3000);
  }

  const body = document.body;
  const navToggle = document.getElementById("mobileNavToggle");
  const sidebar = document.getElementById("appSidebar");
  const backdrop = document.getElementById("mobileNavBackdrop");

  const closeMobileNav = () => {
    body.classList.remove("nav-open");
    if (navToggle) {
      navToggle.setAttribute("aria-expanded", "false");
    }
  };

  if (navToggle && sidebar && backdrop) {
    navToggle.addEventListener("click", () => {
      const isOpen = body.classList.toggle("nav-open");
      navToggle.setAttribute("aria-expanded", isOpen ? "true" : "false");
    });

    backdrop.addEventListener("click", closeMobileNav);

    sidebar.querySelectorAll("a").forEach((link) => {
      link.addEventListener("click", closeMobileNav);
    });

    window.addEventListener("resize", () => {
      if (window.innerWidth > 900) {
        closeMobileNav();
      }
    });

    document.addEventListener("keydown", (event) => {
      if (event.key === "Escape") {
        closeMobileNav();
      }
    });
  }
});
