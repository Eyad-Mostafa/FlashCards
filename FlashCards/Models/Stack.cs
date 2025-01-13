﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Models;

internal class Stack
{
    public int StackId { get; set; }
    public string Name { get; set; }
    public List<Flashcard> Flashcards { get; set; }
}
