const getMembersBoardById = (id, userId) => {
   // request ajax method: GET, url: https://localhost:44372/api/Board/GetMembersBoardById/{id}
   // return ajax method: GET, url: https://localhost:44372/api/Board/GetMembersBoardById/{id}
   return $.ajax({
      type: 'GET',
      url: `https://localhost:5001/api/memberboards/board/${id}`,
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      success: function (response) {
         console.log(response)
         let html = ''
         response.data.map((item, i) => {
            html += `
            <tr>
               <td>${i + 1}</td>
               <td>${item.user.fullName}</td>
               <td>${item.user.email}</td>
               <td>${item.role}</td>
               <td>
               ${
                  item.board.createdBy == userId
                     ? item.role != 'PM'
                        ? `<button class="btn btn-warning mr-2">
                     <i class="fa fa-pencil" aria-hidden="true"></i>
                  </button>
                  <button class="btn btn-outline-danger" onclick="kickMember(${item.id})">
                     kick
                  </button>
                  `
                        : `
                        <button class="btn btn-secondary" disabled>DISABLED</button>
                        `
                     : item.user.id == userId
                     ? `<button class="btn btn-outline-danger" onclick="leaveMember(${item.id})">Leave</button>`
                     : `<button class="btn btn-secondary" disabled>DISABLED</button>`
               }
               </td>
            </tr>
            `
         })

         $('#members').append(html)
      },
      error: function (e) {
         location.href = '/boards'
      },
   })
}

const kickMember = (id) => {
   swal({
      title: 'Are you sure?',
      text: 'Once deleted, you will not be able to recover!',
      icon: 'warning',
      buttons: true,
      dangerMode: true,
   }).then((willDelete) => {
      if (willDelete) {
         $.ajax({
            type: 'DELETE',
            url: `https://localhost:5001/api/memberboards/delete/${id}`,
            success: function (response) {
               location.reload()
            },
         })
         swal('Poof! Your member has been deleted!', {
            icon: 'success',
         })
      } else {
         swal('Your member is safe!')
      }
   })
}

const leaveMember = (id) => {
   swal({
      title: 'Are you sure?',
      text: 'Once deleted, you will not be able to recover!',
      icon: 'warning',
      buttons: true,
      dangerMode: true,
   }).then((willDelete) => {
      if (willDelete) {
         $.ajax({
            type: 'DELETE',
            url: `https://localhost:5001/api/memberboards/delete/${id}`,
            success: function (response) {
               swal('Poof! Your member has been deleted!', {
                  icon: 'success',
               })

               setTimeout(() => {
                  location.href = '/boards'
               }, 1200)
            },
         })
      } else {
         swal('Your member is safe!')
      }
   })
}
