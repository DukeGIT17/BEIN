const stars = document.querySelectorAll('.star');
const linkEl = document.createElement('link');
linkEl.rel = 'stylesheet';
linkEl.href = '/css/public pages css/product.css';
document.head.appendChild(linkEl);


function showMore() {
    const showMore = document.getElementById('show-more');
    const description = document.getElementById('desc');

    if (showMore) {
        if (showMore.innerText === 'SHOW MORE') {
            description.style.height = 'fit-content';
            showMore.innerText = 'SHOW LESS';
        } else {
            description.style.height = '70%';
            showMore.innerText = 'SHOW MORE';
        }
    } else {
        console.alert('Could not find the show more button element.');
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