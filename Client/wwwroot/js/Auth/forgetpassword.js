function SendOTP() {
   event.preventDefault()
   const email = $('#email').val()

   const request = {
      email,
   }

   $.ajax({
      type: 'POST',
      url: 'https://kselleo.eventarry.com/api/accounts/forgotpassword',
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      data: JSON.stringify(request),
      dataType: 'json',
   })
      .done((response) => {
         Swal.fire(
            'Success!',
            'OTP Code is sent to your email address!',
            'success'
         )
         setTimeout(() => {
            window.location.href = 'https://kselleo.eventarry.com/Auth/ChangePassword'
         }, 2000)
      })
      .fail((e) => {
         Swal.fire('Error!', `${JSON.parse(e.responseText).message}`, 'error')
         console.log('e', JSON.parse(e))
      })

   console.log('Send OTP')
}

function ChangePassword() {
   event.preventDefault()
   const Email = $('#email').val()
   const OTP = parseInt($('#otp').val())
   const NewPassword = $('#newpassword').val()
   const ConfirmPassword = $('#confirmpassword').val()

   if (NewPassword != ConfirmPassword) {
      $('#error-confirmPassword').html('Value do not match!')
      return
   }

   const request = {
      Email,
      OTP,
      NewPassword,
      ConfirmPassword,
   }

   $.ajax({
      type: 'POST',
      url: 'https://kselleo.eventarry.com/api/accounts/changepassword',
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      data: JSON.stringify(request),
      dataType: 'json',
   })
      .done((response) => {
         Swal.fire('Success!', 'Change Password Successfully!', 'success')
         setTimeout(() => {
            window.location.href = 'https://kselleo.eventarry.com/Auth/Login'
         }, 2000)
      })
      .fail((e) => {
         Swal.fire('Error!', `${JSON.parse(e.responseText).message}`, 'error')
         console.log('e', JSON.parse(e))
      })

   $('#error-confirmPassword').html('')

   console.log('Change Password', JSON.stringify(request))
}
