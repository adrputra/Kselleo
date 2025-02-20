const getPendingMembers = (boardId) => {
   console.log(boardId)

   // get data method GET url https://localhost:8999/api/verifyinvites/pending/{boardId} by ajax
   $.ajax({
      url: `https://localhost:8999/api/verifyinvites/pending/board/${boardId}`,
      type: 'GET',
      dataType: 'json',
      success: function (response) {
         var html = ''

         if (response.data.length) {
            response.data.forEach((element, i) => {
               html += `
            <tr>
               <td>${i + 1}</td>
               <td>${element.user.fullName}</td>
               <td>${element.user.email}</td>
               <td>${element.role}</td>
               <td>
               <span class="badge badge-warning">PENDING</span>
               </td>
            </tr>
            `
            })
         } else {
            html += `
            <tr>
               <td colspan="5" class="text-center">Pending invitation not found</td>
            </tr>
            `
         }

         console.log('html', html)

         $('#bodyTable').html(html)
      },
      error: function (err) {
         console.log(err)
      },
   })
}
