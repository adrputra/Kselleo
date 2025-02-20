function Login() {
   event.preventDefault()
   const email = $('#email').val()
   const password = $('#password').val()

   const request = {
      email,
      password,
   }

   $.ajax({
      type: 'POST',
      url: 'https://kselleo.eventarry.com/api/accounts/login',
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      data: JSON.stringify(request),
      dataType: 'json',
   })
      .done((response) => {
         Swal.fire('Success!', 'Your login success', 'success')
         setTimeout(() => {
            window.location.href = 'https://kselleo.eventarry.com/Admin/Dashboard'
         }, 2000)
      })
      .fail((e) => {
         Swal.fire('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      })

   $('#error-confirmPassword').html('')

   console.log('Login')
}
