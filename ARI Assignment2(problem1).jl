### A Pluto.jl notebook ###
# v0.19.2

using Markdown
using InteractiveUtils

# ╔═╡ 11f85da5-0269-4c65-ad66-f7064ed9ce83
using Pkg

# ╔═╡ d245e868-2956-4de5-8d93-1054c88c3a44
Pkg.activate("Project.toml")

# ╔═╡ ed43a5ef-4c6a-450d-8831-5495963543cf
Pkg.add("DataStructures")

# ╔═╡ be5ccac7-29aa-4dbd-a96b-1c7eb54ed85c
begin
	Pkg.add("PlutoUI")
using PlutoUI
end

# ╔═╡ 217c4734-c945-4fd3-ab88-9424daef4079
begin
	Pkg.add("AStarSearch")
	using AStarSearch
end

# ╔═╡ be1dc32d-da4d-4250-8eb8-025058609202
begin
using Markdown
using InteractiveUtils
end

# ╔═╡ 6dadc240-222f-4c3d-a533-7ef37c27e525
using DataStructures

# ╔═╡ 739cf4a4-eb7f-4eac-b055-a19cacbbcbfc
using Test

# ╔═╡ ae3ea2c2-cc77-11ec-14fb-83acc708175b
md"# Assignment 1"

# ╔═╡ ef86eb6f-a636-4db7-b53c-a00210872eb9
md"## Problem 1"

# ╔═╡ ce9630f9-1242-443a-b525-de08d389a80e
struct Action
	Name::String
	cost::Int64
end

# ╔═╡ f6c9f366-2862-468b-a028-64169d9923a2
move_up = Action("Move Up",1)

# ╔═╡ 18fb7572-7797-4467-b211-0ec3b5eca33b
move_down = Action("Move Down",1)

# ╔═╡ 118157b5-1071-4e59-b43e-6207d7b6b7d5
move_right = Action("Move Right",2)

# ╔═╡ 682437d8-7a83-489c-8ee4-9b2ffc06e34a
move_left = Action("Move Left",2)

# ╔═╡ a88378a6-8154-4ba4-894f-8a42efaf3163
co = Action("Collect",5)

# ╔═╡ 3b358a08-1b78-4f50-ad13-c2c7d8b6c4fc
struct State
	Name::String 
	hasItem::Vector{Bool}
	OfficeLocation::Int64
end

# ╔═╡ 6726d609-a3ab-47e5-9789-8f7b78ef62d0
state1 = State("State 1", [true,true,true],1)

# ╔═╡ 6d9a2f07-fd39-4af5-824a-746eba25de10
state2 = State("State 2",[true, true,false],2)

# ╔═╡ 4d17aeef-e226-46ec-9b96-6f2fe1b6e82d
state3 =State("State 3",[true,true,true],3)

# ╔═╡ cd931f06-9303-4aa1-91e9-f373c6efaa58
state4= State("State 4",[true,false,true],1)

# ╔═╡ 3ac6c8a2-8048-4393-b858-3be4ef0203ff
state5 = State("State 5",[true,true,true],2)

# ╔═╡ f3ece652-4340-45b9-8edd-854870e6e88f
state6 = State("State 6",[true,false,false],3)

# ╔═╡ 250b35dc-8e4e-4f28-b751-d969017059a0
state7 = State("State 7",[true,false,false],1)

# ╔═╡ 9848a95a-e08a-4631-8b08-41a9f5f08002
state8 = State("State 8",[true,false,true],2)

# ╔═╡ b1291422-b9cf-4b5c-8a5e-65561b02eab9
state9 = State("State 9",[false,false,true],3)

# ╔═╡ a9b9d117-fcf1-42f8-82ef-a6f09b77e34a
state10 = State("State 10",[false,true,false],1)

# ╔═╡ fa87dfc5-433f-4f35-af34-e54399e15610
state11 = State("State 11",[false,false,false],2)

# ╔═╡ 4630c559-eb8e-4d55-b9a1-e24649cccce3
goalstates = [state11]

# ╔═╡ fdb56163-ff9b-4efb-85f5-c724a3d3302f
TransitionModel = Dict(state1 => [(move_left ,state2),(co,state1)])

# ╔═╡ 23474ebc-9553-4cdf-854b-675f2bae9b5b
push!(TransitionModel, state2 => [(co,state2),(move_left, state3)])

# ╔═╡ d00173b9-81a6-489f-b842-d9ba8dc1cefa
push!(TransitionModel , state3 => [(co,state3),(move_down,state4)])

# ╔═╡ 03b7c435-7555-49be-a6e1-1a1485d52d4e
push!(TransitionModel , state4 => [(co,state4),(move_left,state3)])

# ╔═╡ 9a15947f-f9ef-4557-907b-df671d879a33
push!(TransitionModel , state5 => [(co,state5),(move_left,state4)])

# ╔═╡ de9554ee-b6c6-42e6-b717-9fe4c96d5aa4
push!(TransitionModel , state6 => [(co,state6),(move_left,state5)])

# ╔═╡ 8f1af488-efe8-427e-9d33-5272c18fd197
push!(TransitionModel , state7 => [(co,state7),(move_down,state8)])

# ╔═╡ 845655b1-9ec3-40cd-a954-12670bef697e
push!(TransitionModel , state8 => [(move_right, state9),(co,state8)])

# ╔═╡ b602f2b1-485d-4260-8b64-d6c12efb8beb
push!(TransitionModel , state9 => [(co,state9),(move_right,state10)])


# ╔═╡ 39f731ca-18b5-4a54-9063-06e0f37c3411
push!(TransitionModel , state10 => [(co,state10),(move_down,state11)])

# ╔═╡ 93b238b5-05e2-4e86-bc1c-2ad20df26b14
push!(TransitionModel , state11 => [(move_up,state10)])

# ╔═╡ a65cc66f-101e-4735-be72-e8fc79908a56
TransitionModel[state1]

# ╔═╡ 5eac326c-b932-4e22-836f-668a7abacbd0
function AStar(state1, TransitionModel, goalstates,clean,removeItem)
	explored = []
	beginState = Tmodel{State, State}()
	initial = state1
	
	while true
		if isempty(office)
			return []
		else
			(dirt, noDirt) = clean(office)
			current_state = dirt
			office = noDirt
			
			push!(explored, current_state)
			office_Part = TransitionModel[current_state]
			for oneOffice in office_Part
				if !((oneOffice[2]+cost) in explored)
					push!(beginState, oneOffice[2]+cost => current_state)
					if (goalstates(oneOffice[2]+cost))
						return create_result(TransitionModel, beginState,
						state1, oneOffice[2]+cost)
					else
						office_Part = removeDirt(office_Part,
						oneOffice[2]+cost, oneOffice[1].cost)
					end
				end
			end
		end
	end
end

# ╔═╡ afe2f387-13d4-4cad-ba82-2e9bdbb804ca
AStar(state1, TransitionModel, goalstates,clean,removeItem)

# ╔═╡ Cell order:
# ╠═ae3ea2c2-cc77-11ec-14fb-83acc708175b
# ╠═ef86eb6f-a636-4db7-b53c-a00210872eb9
# ╠═be1dc32d-da4d-4250-8eb8-025058609202
# ╠═11f85da5-0269-4c65-ad66-f7064ed9ce83
# ╠═d245e868-2956-4de5-8d93-1054c88c3a44
# ╠═ed43a5ef-4c6a-450d-8831-5495963543cf
# ╠═be5ccac7-29aa-4dbd-a96b-1c7eb54ed85c
# ╠═6dadc240-222f-4c3d-a533-7ef37c27e525
# ╠═739cf4a4-eb7f-4eac-b055-a19cacbbcbfc
# ╠═217c4734-c945-4fd3-ab88-9424daef4079
# ╠═ce9630f9-1242-443a-b525-de08d389a80e
# ╠═f6c9f366-2862-468b-a028-64169d9923a2
# ╠═18fb7572-7797-4467-b211-0ec3b5eca33b
# ╠═118157b5-1071-4e59-b43e-6207d7b6b7d5
# ╠═682437d8-7a83-489c-8ee4-9b2ffc06e34a
# ╠═a88378a6-8154-4ba4-894f-8a42efaf3163
# ╠═3b358a08-1b78-4f50-ad13-c2c7d8b6c4fc
# ╠═6726d609-a3ab-47e5-9789-8f7b78ef62d0
# ╠═6d9a2f07-fd39-4af5-824a-746eba25de10
# ╠═4d17aeef-e226-46ec-9b96-6f2fe1b6e82d
# ╠═cd931f06-9303-4aa1-91e9-f373c6efaa58
# ╠═3ac6c8a2-8048-4393-b858-3be4ef0203ff
# ╠═f3ece652-4340-45b9-8edd-854870e6e88f
# ╠═250b35dc-8e4e-4f28-b751-d969017059a0
# ╠═9848a95a-e08a-4631-8b08-41a9f5f08002
# ╠═b1291422-b9cf-4b5c-8a5e-65561b02eab9
# ╠═a9b9d117-fcf1-42f8-82ef-a6f09b77e34a
# ╠═fa87dfc5-433f-4f35-af34-e54399e15610
# ╠═4630c559-eb8e-4d55-b9a1-e24649cccce3
# ╠═fdb56163-ff9b-4efb-85f5-c724a3d3302f
# ╠═23474ebc-9553-4cdf-854b-675f2bae9b5b
# ╠═d00173b9-81a6-489f-b842-d9ba8dc1cefa
# ╠═03b7c435-7555-49be-a6e1-1a1485d52d4e
# ╠═9a15947f-f9ef-4557-907b-df671d879a33
# ╠═de9554ee-b6c6-42e6-b717-9fe4c96d5aa4
# ╠═8f1af488-efe8-427e-9d33-5272c18fd197
# ╠═845655b1-9ec3-40cd-a954-12670bef697e
# ╠═b602f2b1-485d-4260-8b64-d6c12efb8beb
# ╠═39f731ca-18b5-4a54-9063-06e0f37c3411
# ╠═93b238b5-05e2-4e86-bc1c-2ad20df26b14
# ╠═a65cc66f-101e-4735-be72-e8fc79908a56
# ╠═5eac326c-b932-4e22-836f-668a7abacbd0
# ╠═afe2f387-13d4-4cad-ba82-2e9bdbb804ca
