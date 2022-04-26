const register = () => {
   event.preventDefault()
   const fullName = $('#fullName').val()
   const email = $('#email').val()
   const gender = $("input[name='gender']:checked").val()
   const password = $('#password').val()
   const confirmPassword = $('#confirmPassword').val()

   if (password != confirmPassword) {
      $('#error-confirmPassword').html('Value do not match!')
      return
   }

   const request = {
      fullName,
      email,
      gender,
      password,
   }

   $.ajax({
      type: 'POST',
      url: 'https://localhost:5001/api/accounts/register',
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      data: JSON.stringify(request),
      dataType: 'json',
      // success: function (response, asd) {
      //    swal('Success!', 'Your account has been registered!', 'success')
      //    console.log('SUCCESSSS')
      //    // window.location.href = 'https://localhost:3000/auth/login'
      // },
      // error: function (e, asd) {
      //    swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
      //    console.log('e', JSON.parse(e.responseText))
      // },
      // }).done((response) => {
      //    console.log('OKKKKKK')
      //    swal('Success!', 'Your account has been registered!', 'success')
      //    window.location.href = 'https://localhost:3000/auth/login'
   })
      .done((response) => {
         swal('Success!', 'Your account has been registered!', 'success')
         setTimeout(() => {
            window.location.href = 'https://localhost:3000/auth/login'
         }, 2000)
      })
      .fail((e) => {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
         console.log('e', JSON.parse(e))
      })

   $('#error-confirmPassword').html('')

   console.log('insert data')
}
