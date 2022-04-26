function Login() {
    event.preventDefault();
    const email = $('#email').val()
    const password = $('#password').val()

    const request = {
        email,
        password
    }

    $.ajax({
        type: 'POST',
        url: 'https://localhost:44308/api/accounts/login',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
        },
        data: JSON.stringify(request),
        dataType: 'json',
        success: function (response, asd) {
            Swal.fire({
                icon: 'success',
                title: 'SUCCESS',
            })
            window.location.href = 'https://localhost:44346/Admin/Dashboard'
        },
        error: function (e, asd) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!',
            })
        },
    })
}