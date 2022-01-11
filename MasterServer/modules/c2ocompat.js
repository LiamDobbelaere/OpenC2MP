/*
    C2O Compatibility Module

    OpenC2MP - Open Carmageddon 2 Multiplayer
    Copyright (C) 2022 Tom Dobbelaere

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.

    Contact the author at: tom.dobbelaere@outlook.com
*/

const path = require('path');
const express = require('express');

const c2oRouter = express();

// RAM-only, no need for DB
let serverList = [];

c2oRouter.use('/C2O_DATA', express.static(path.join(__dirname, '..', 'public', 'C2O_DATA')));

c2oRouter.get('/whatismyip.php', (req, res) => {
  // surprisingly, C2O depends on the fact that TITLE is on a separate line and capitalized
  res.send(`<HTML>
<HEAD>
<TITLE>${req.ip}</TITLE>
</HEAD>
<BODY>
</BODY>
</HTML>`);
});

c2oRouter.get('/update_servers.php', (req, res) => {
  const { action, name, ip, mode, peds, drones, recovery, players, laps, timelimit } = req.query;

  switch (action) {
    case 'add':
      serverList.push({
        name,
        ip,
        mode,
        peds,
        drones,
        recovery,
        players
      });
      res.send('you have been added to the master server list');
      break;
    case 'rem':
      try {
        serverList = serverList.filter(server => server.ip === ip);
        res.send('you have been removed from the master server list');
      } catch (err) {
        console.error(err);
        res.send('Error updating master server list');
      }
      break;
    case 'edit':
      const serverIdx = serverList.findIndex(server => server.ip === ip);
      if (serverIdx === -1) {
        res.send('Error updating master server list');
        return;
      }

      serverList[serverIdx] = {
        ...serverList[serverIdx],
        mode,
        peds,
        drones,
        recovery,
        players
      };
      res.send('your server information has been updated successfully');
      break;
    case 'get':
      // seems to be unused
      const result = serverList.reduce((acc, server) => {
        return acc + `*${server.name} @ ${server.ip}\r\n`;
      }, '');
      res.send(result);
      break;
    default:
      res.send('');
  }
});

module.exports = c2oRouter;
