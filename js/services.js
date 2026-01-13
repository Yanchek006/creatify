// Services tab switching
document.addEventListener('DOMContentLoaded', function() {
    const tabs = document.querySelectorAll('.service-tab');
    const categories = document.querySelectorAll('.service-category');
    
    // URL hash handling
    function activateTabFromHash() {
        const hash = window.location.hash.substring(1);
        if (hash) {
            const targetTab = document.querySelector(`.service-tab[data-category="${hash}"]`);
            if (targetTab) {
                activateTab(targetTab);
            }
        }
    }
    
    function activateTab(tab) {
        // Remove active class from all tabs
        tabs.forEach(t => t.classList.remove('active'));
        
        // Add active class to clicked tab
        tab.classList.add('active');
        
        // Get category
        const category = tab.getAttribute('data-category');
        
        // Hide all categories
        categories.forEach(cat => {
            cat.classList.remove('active');
        });
        
        // Show selected category
        const targetCategory = document.getElementById(category);
        if (targetCategory) {
            targetCategory.classList.add('active');
        }
        
        // Update URL hash
        window.location.hash = category;
    }
    
    // Tab click events
    tabs.forEach(tab => {
        tab.addEventListener('click', function(e) {
            e.preventDefault();
            activateTab(this);
        });
    });
    
    // Initialize from hash or first tab
    if (window.location.hash) {
        activateTabFromHash();
    } else {
        tabs[0].click();
    }
    
    // Smooth scroll for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function(e) {
            if (this.hash) {
                e.preventDefault();
                const target = document.querySelector(this.hash);
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
        });
    });
});