// sweetalert.js

// SweetAlert for Successful Login
function showSuccessLogin() {
    Swal.fire({
        icon: 'success',
        title: 'Logged In Successfully!',
        text: 'Welcome to the Quiz Management System',
        background: '#fff3cd',
        confirmButtonColor: '#3085d6',
        timer: 3000
    });
}

// SweetAlert for Logout Confirmation
function confirmLogout() {
    return Swal.fire({
        title: 'Are you sure you want to log out?',
        text: "You will need to log in again to continue.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, log out!'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = "/User/Logout";
        }
    });
}

// SweetAlert for Successful Logout
function showSuccessLogout() {
    Swal.fire({
        icon: 'success',
        title: 'Logged Out Successfully!',
        text: 'You have been logged out.',
        background: '#fff3cd',
        confirmButtonColor: '#3085d6',
        timer: 3000
    });
}
