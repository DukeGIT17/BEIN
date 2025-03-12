const linkEl = document.createElement('link');
const responsiveLinkEl = document.createElement('link');

linkEl.rel = 'stylesheet';
linkEl.href = '/css/public pages css/product.css';

responsiveLinkEl.rel = 'stylesheet';
responsiveLinkEl.href = '/css/public pages css/responsive product.css';

document.head.appendChild(linkEl);
document.head.appendChild(responsiveLinkEl);


const stars = document.querySelectorAll('.star');
const showMoreBtn = document.getElementById('show-more');
const description = document.getElementById('desc');

document.addEventListener('DOMContentLoaded', () => {
    if (description.clientHeight <= 150) {
        showMoreBtn.style.display = 'none';
    }
});

function showMore() {
    if (showMoreBtn) {
        if (showMoreBtn.innerText === 'SHOW MORE') {
            description.style.maxHeight = '50vh';
            description.style.overflow = 'auto';
            showMoreBtn.innerText = 'SHOW LESS';
        } else {
            description.style.maxHeight = '30vh';
            description.style.overflow = 'hidden';
            showMoreBtn.innerText = 'SHOW MORE';
        }
    } else {
        alert('Could not find the show more button element.');
    }
}

stars.forEach((star, index) => {
    star.addEventListener('mouseover', () => {
        for (let i = 0; i <= index; i++) {
            stars[i].classList.add('star-hover');
        }
    });

    star.addEventListener('mouseleave', () => {
        stars.forEach(star => {
            star.classList.remove('star-hover');
        });
    });

    star.addEventListener('click', (event) => {
        const radioOption = document.getElementById(event.target.getAttribute('data-target'));
        radioOption.click();
        stars.forEach(star => {
            star.classList.remove('star-hover');
        });

        if (stars[index].classList.contains('star-selected')) {
            for (let i = index + 1; i < 5; i++) {
                stars[i].classList.remove('star-selected');
                stars[i].classList.replace('fa-solid', 'fa-regular');
            }
        } else {
            for (let i = 0; i <= index; i++) {
                stars[i].classList.add('star-selected');
                stars[i].classList.replace('fa-regular', 'fa-solid');
            }
        }

    })
});

function areAllStarsSelected() {
    let allStarsSelected = true;

    stars.forEach(star => {
        if (!star.classList.contains('star-selected')) {
            allStarsSelected = false;
        }
    })

    return allStarsSelected;
}