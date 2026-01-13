class LanguageSwitcher {
    constructor() {
        this.currentLang = 'ru';
        this.elements = [];
        this.init();
    }
    
    init() {
        // Get all language buttons
        const langButtons = document.querySelectorAll('.lang-btn');
        langButtons.forEach(btn => {
            btn.addEventListener('click', () => {
                const lang = btn.getAttribute('data-lang');
                this.switchLanguage(lang);
            });
        });
        
        // Find all elements with language attributes
        this.findLanguageElements();
        
        // Check for saved language preference
        const savedLang = localStorage.getItem('preferred-language');
        if (savedLang) {
            this.switchLanguage(savedLang);
        }
    }
    
    findLanguageElements() {
        // Find elements with data-lang attributes
        const elements = document.querySelectorAll('[data-lang-ru], [data-lang-en]');
        elements.forEach(el => {
            this.elements.push({
                element: el,
                ru: el.getAttribute('data-lang-ru'),
                en: el.getAttribute('data-lang-en')
            });
        });
    }
    
    switchLanguage(lang) {
        this.currentLang = lang;
        
        // Update button states
        document.querySelectorAll('.lang-btn').forEach(btn => {
            if (btn.getAttribute('data-lang') === lang) {
                btn.classList.add('active');
            } else {
                btn.classList.remove('active');
            }
        });
        
        // Update all language elements
        this.elements.forEach(item => {
            if (item[lang]) {
                if (item.element.tagName === 'INPUT' || item.element.tagName === 'TEXTAREA') {
                    item.element.placeholder = item[lang];
                } else {
                    item.element.textContent = item[lang];
                }
            }
        });
        
        // Update HTML lang attribute
        document.documentElement.setAttribute('lang', lang);
        
        // Save preference
        localStorage.setItem('preferred-language', lang);
        
        // Dispatch event for other components
        document.dispatchEvent(new CustomEvent('languageChanged', { detail: lang }));
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new LanguageSwitcher();
});