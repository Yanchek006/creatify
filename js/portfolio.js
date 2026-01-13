// Portfolio filtering and lightbox
document.addEventListener('DOMContentLoaded', function() {
    const filterButtons = document.querySelectorAll('.filter-btn');
    const workItems = document.querySelectorAll('.work-item');
    
    // Filter works
    filterButtons.forEach(button => {
        button.addEventListener('click', function() {
            // Remove active class from all buttons
            filterButtons.forEach(btn => btn.classList.remove('active'));
            
            // Add active class to clicked button
            this.classList.add('active');
            
            const filter = this.getAttribute('data-filter');
            
            // Show/hide work items
            workItems.forEach(item => {
                if (filter === 'all' || item.getAttribute('data-category') === filter) {
                    item.style.display = 'block';
                    setTimeout(() => {
                        item.style.opacity = '1';
                        item.style.transform = 'translateY(0)';
                    }, 100);
                } else {
                    item.style.opacity = '0';
                    item.style.transform = 'translateY(20px)';
                    setTimeout(() => {
                        item.style.display = 'none';
                    }, 300);
                }
            });
            
            // Show/hide category sections
            const categories = document.querySelectorAll('.portfolio-category');
            categories.forEach(category => {
                if (filter === 'all') {
                    category.style.display = 'block';
                } else {
                    const categoryId = category.id.replace('-works', '');
                    if (categoryId === filter) {
                        category.style.display = 'block';
                    } else {
                        category.style.display = 'none';
                    }
                }
            });
        });
    });
    
    // Lightbox functionality
    const lightbox = document.getElementById('lightbox');
    const lightboxImg = lightbox.querySelector('.lightbox-image img');
    const lightboxTitle = lightbox.querySelector('.lightbox-info h3');
    const lightboxDesc = lightbox.querySelector('.lightbox-info p');
    const lightboxClose = lightbox.querySelector('.lightbox-close');
    
    // Sample work data (in real project, this would come from database)
    const workData = {
        'ai-1': {
            title: 'Нейро-арт "Космос"',
            description: 'Генерация изображений с помощью искусственного интеллекта Midjourney с последующей ручной доработкой в Photoshop.',
            details: ['AI генерация', 'Ручная доработка', '4K разрешение']
        },
        'website-1': {
            title: 'Интернет-магазин "Moda"',
            description: 'Полный цикл разработки интернет-магазина модной одежды на платформе Tilda.',
            details: ['Tilda', 'E-commerce', 'Мобильная адаптация']
        }
        // Add more work data
    };
    
    // Open lightbox
    document.querySelectorAll('.view-details').forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            const workItem = this.closest('.work-item');
            const workId = workItem.querySelector('img').alt.replace(/\s+/g, '-').toLowerCase();
            
            if (workData[workId]) {
                lightboxImg.src = workItem.querySelector('img').src;
                lightboxTitle.textContent = workData[workId].title;
                lightboxDesc.textContent = workData[workId].description;
                
                // Clear and add details
                const detailsContainer = lightbox.querySelector('.lightbox-details');
                detailsContainer.innerHTML = '';
                workData[workId].details.forEach(detail => {
                    const tag = document.createElement('span');
                    tag.className = 'detail-tag';
                    tag.textContent = detail;
                    detailsContainer.appendChild(tag);
                });
                
                lightbox.classList.add('active');
                document.body.style.overflow = 'hidden';
            }
        });
    });
    
    // Close lightbox
    lightboxClose.addEventListener('click', function() {
        lightbox.classList.remove('active');
        document.body.style.overflow = 'auto';
    });
    
    // Close lightbox on background click
    lightbox.addEventListener('click', function(e) {
        if (e.target === lightbox) {
            lightbox.classList.remove('active');
            document.body.style.overflow = 'auto';
        }
    });
    
    // Close lightbox on ESC key
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape' && lightbox.classList.contains('active')) {
            lightbox.classList.remove('active');
            document.body.style.overflow = 'auto';
        }
    });
});