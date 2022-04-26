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
    }).done((response) => {
        swal('Success!', 'Your account has been registered!', 'success')
        setTimeout(() => {
            window.location.href = 'https://localhost:44346/Admin/Dashboard'
        }, 2000)
    }).fail((e) => {
        swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
        console.log('e', JSON.parse(e))
    })

    $('#error-confirmPassword').html('')

    console.log('Login')
}