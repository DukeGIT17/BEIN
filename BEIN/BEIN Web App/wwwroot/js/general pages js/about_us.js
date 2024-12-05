document.addEventListener('DOMContentLoaded', (event) => {
    // Only keeping the news card functionality
    const cards = document.querySelectorAll('.news-card');
    cards.forEach(card => {
        const readMoreBtn = card.querySelector('.read-more');
        readMoreBtn.addEventListener('click', (e) => {
            e.preventDefault();
            card.classList.toggle('expanded');
            if (card.classList.contains('expanded')) {
                readMoreBtn.textContent = 'Read Less';
            } else {
                readMoreBtn.textContent = 'Read More';
            }
        });
    });
});