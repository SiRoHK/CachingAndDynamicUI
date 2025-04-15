document.addEventListener('DOMContentLoaded', () => {
    const usersContainer = document.getElementById('users-container');
    const userTemplate = document.getElementById('user-template');
    const loader = document.getElementById('loader');
    const errorMessage = document.getElementById('error-message');
    const searchInput = document.getElementById('search');
    const refreshBtn = document.getElementById('refresh-btn');

    let users = [];

    // Load users on page load
    loadUsers();

    // Set up event listeners
    searchInput.addEventListener('input', filterUsers);
    refreshBtn.addEventListener('click', loadUsers);

    async function loadUsers() {
        showLoader();
        hideError();
        clearUsers();

        try {
            const response = await fetch('/api/users');

            if (!response.ok) {
                throw new Error('Failed to fetch users');
            } getImageUrl
            users = await response.json();
            displayUsers(users);
        } catch (error) {
            showError(error.message);
            console.error('Error fetching users:', error);
        } finally {
            hideLoader();
        }
    }

    function getImageUrl(userId) {
        // Check if browser supports WebP
        const supportsWebP = localStorage.getItem('supportsWebP');

        if (supportsWebP === 'true') {
            return `https://picsum.photos/seed/${userId}/400/400.webp`;
        } else if (supportsWebP === 'false') {
            return `https://picsum.photos/seed/${userId}/400/400`;
        } else {
            // Test WebP support if we haven't checked before
            const webP = new Image();
            webP.onload = function () {
                localStorage.setItem('supportsWebP', 'true');
            };
            webP.onerror = function () {
                localStorage.setItem('supportsWebP', 'false');
            };
            webP.src = 'data:image/webp;base64,UklGRh4AAABXRUJQVlA4TBEAAAAvAAAAAAfQ//73v/+BiOh/AAA=';

            // Return non-WebP for now
            return `https://picsum.photos/seed/${userId}/400/400`;
        }
    }

    function setupIntersectionObserver() {
        if ('IntersectionObserver' in window) {
            const imageObserver = new IntersectionObserver((entries, observer) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        const img = entry.target;
                        const dataSrc = img.getAttribute('data-src');

                        if (dataSrc) {
                            img.src = dataSrc;
                            img.removeAttribute('data-src');
                        }

                        observer.unobserve(img);
                    }
                });
            }, {
                rootMargin: '50px 0px',
                threshold: 0.01
            });

            // Observe all avatar images
            document.querySelectorAll('.avatar[data-src]').forEach(img => {
                imageObserver.observe(img);
            });
        }
    }

    function displayUsers(usersToDisplay) {
        clearUsers();

        usersToDisplay.forEach((user, index) => {
            const userElement = document.importNode(userTemplate.content, true);

            // Set avatar image with placeholder
            const avatarImg = userElement.querySelector('.avatar');
            avatarImg.src = getImageUrl(user.id);
            avatarImg.setAttribute('data-src', getImageUrl(user.id));
            avatarImg.src = 'data:image/svg+xml,%3Csvg xmlns="http://www.w3.org/2000/svg" width="400" height="400" viewBox="0 0 400 400"%3E%3Crect width="400" height="400" fill="%23f0f0f0"/%3E%3C/svg%3E';

            avatarImg.src = `https://picsum.photos/seed/${user.id}/400/400`;
            avatarImg.alt = `${user.name} avatar`;

            // Set user details
            userElement.querySelector('.user-name').textContent = user.name;
            userElement.querySelector('.user-username').textContent = `@${user.username}`;
            userElement.querySelector('.user-email').textContent = `Email: ${user.email}`;
            userElement.querySelector('.user-company').textContent = `Company: ${user.company.name}`;
            userElement.querySelector('.user-address').textContent =
                `${user.address.street}, ${user.address.city}, ${user.address.zipcode}`;

            const websiteLink = userElement.querySelector('.user-website');
            websiteLink.href = `https://${user.website}`;
            websiteLink.textContent = user.website;

            userElement.querySelector('.user-phone').textContent = user.phone;

            // Add animation delay for a staggered effect
            const userCard = userElement.querySelector('.user-card');
            userCard.style.animationDelay = `${index * 0.1}s`;

            usersContainer.appendChild(userElement);
        });
        setupIntersectionObserver();

    }

    function filterUsers() {
        const searchTerm = searchInput.value.toLowerCase();

        if (!searchTerm) {
            displayUsers(users);
            return;
        }

        const filteredUsers = users.filter(user =>
            user.name.toLowerCase().includes(searchTerm) ||
            user.username.toLowerCase().includes(searchTerm) ||
            user.email.toLowerCase().includes(searchTerm) ||
            user.company.name.toLowerCase().includes(searchTerm)
        );

        displayUsers(filteredUsers);
    }

    function clearUsers() {
        usersContainer.innerHTML = '';
    }

    function showLoader() {
        loader.style.display = 'block';
    }

    function hideLoader() {
        loader.style.display = 'none';
    }

    function showError(message) {
        errorMessage.textContent = message;
        errorMessage.style.display = 'block';
    }

    function hideError() {
        errorMessage.style.display = 'none';
    }
});