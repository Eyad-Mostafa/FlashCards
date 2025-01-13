using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Models;

internal class StackDTO
{
    public string Name { get; set; }
    public List<FlashcardDTO> Flashcards { get; set; }
}
