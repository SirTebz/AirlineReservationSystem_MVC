// Airline Reservation System - Client-side JavaScript

// Auto-dismiss alerts after 5 seconds
document.addEventListener('DOMContentLoaded', function () {
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
});

// Form validation helper
function validateForm(formId) {
    const form = document.getElementById(formId);
    if (form) {
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    }
}

// Date picker minimum date setter
function setMinDateToToday() {
    const dateInputs = document.querySelectorAll('input[type="date"]');
    const today = new Date().toISOString().split('T')[0];
    dateInputs.forEach(input => {
        input.setAttribute('min', today);
    });
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', function () {
    setMinDateToToday();
});

// Loading spinner
function showLoading() {
    const spinner = document.createElement('div');
    spinner.className = 'spinner-border text-primary position-fixed top-50 start-50';
    spinner.id = 'loading-spinner';
    spinner.innerHTML = '<span class="visually-hidden">Loading...</span>';
    document.body.appendChild(spinner);
}

function hideLoading() {
    const spinner = document.getElementById('loading-spinner');
    if (spinner) {
        spinner.remove();
    }
}

// Confirm delete actions
function confirmDelete(message) {
    return confirm(message || 'Are you sure you want to delete this item?');
}

// Format currency
function formatCurrency(amount) {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
    }).format(amount);
}

// Smooth scroll to top
function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}

// Add scroll to top button
document.addEventListener('DOMContentLoaded', function () {
    window.addEventListener('scroll', function () {
        const scrollBtn = document.getElementById('scroll-top-btn');
        if (window.pageYOffset > 300) {
            if (scrollBtn) scrollBtn.style.display = 'block';
        } else {
            if (scrollBtn) scrollBtn.style.display = 'none';
        }
    });
});
