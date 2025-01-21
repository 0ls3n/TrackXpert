

mobileMenuButton = document.querySelector('[aria-controls="mobile-menu"]');
mobileMenu = document.getElementById('mobile-menu');


hamburgerIcon = mobileMenuButton.querySelector('svg.block');
closeIcon = mobileMenuButton.querySelector('svg.hidden');

mobileMenuButton.addEventListener('click', () => {

    mobileMenu.classList.toggle('hide');

    hamburgerIcon.classList.toggle('hidden');
    closeIcon.classList.toggle('hidden');
});



userMenuButton = document.getElementById('user-menu-button');
userMenu = document.querySelector('[aria-labelledby="user-menu-button"]');

userMenuButton.addEventListener('click', () => {
    userMenu.classList.toggle('hide');
});


