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
      dataType: 'dataType',
      success: function (response, asd) {
         swal('Success!', 'Your account has been registered!', 'success')

      },
      error: function (e, asd) {
         swal('Error!', `${JSON.parse(e.responseText).message}`, 'error')
         console.log('e', JSON.parse(e.responseText))
      },
   })

   $('#error-confirmPassword').html('')

   console.log('insert data')
}
