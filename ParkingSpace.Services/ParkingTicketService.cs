﻿using ParkingSpace.Models;
using ParkingSpace.Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSpace.Services {
  public class ParkingTicketService {
    public int GateId { get; set; }
    public int NextId { get; set; }

    public ParkingTicketService() {
      GateId = 0;
      NextId = 1;
    }

    public ParkingTicket CreateParkingTicket(string plateNo) {

      var ticket = new ParkingTicket();

      ticket.PlateNumber = plateNo;
      ticket.DateIn = SystemTime.Now();
      ticket.Id = generateId();
      ticket.GateId = GateId;

      return ticket;
    }

    private string generateId() {
      string s = $"{GateId:00}-{NextId:00000}"; //string interpolation
      this.NextId++;
      return s;
    }



  }
}
