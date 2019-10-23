using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;

namespace D_Squared.Web.Models
{
    public class MeetingNotesBaseViewModel
    {
        public MeetingNotesBaseViewModel()
        {
            NotesDTO = new MeetingNotesDTO();
        }
        public MeetingNotesBaseViewModel(MeetingNotesDTO notesDTO)
        {
            NotesDTO = notesDTO;
        }

        public MeetingNotesDTO NotesDTO { get; set; }
        public EmployeeDTO EmployeeInfo { get; set; }

        public string GetManagerName()
        {
            return $"{this.EmployeeInfo.FirstName} {this.EmployeeInfo.LastName}";
        }

        public string GetHuddleDate()
        {
            return $"{this.NotesDTO.HuddleDate.Value.ToShortDateString()} {this.NotesDTO.HuddleDate.Value.ToShortTimeString()}";
        }
    }
    public class MeetingNotesListViewModel
    {
        public List<MeetingNotesDTO> NotesDTOList { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        public DateTime BusinessWeekStartDate { get; set; }

        public DateTime BusinessWeekEndDate { get; set; }

        public bool CurrentWeekFlag { get; set; }
    }
}