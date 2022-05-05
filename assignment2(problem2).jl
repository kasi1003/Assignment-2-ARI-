### A Pluto.jl notebook ###
# v0.19.2

using Markdown
using InteractiveUtils

# ╔═╡ 2b466abb-1e66-4d89-9565-f56b4f82e9d5


# ╔═╡ 39971b20-ca63-11ec-07e5-d14038fe071a
#actions 
function responce_time(outbound_latency, processing_time, imbound_latency)
	responce_time_return = outbound_latency + processing_time + imbound_latency
	return responce_time_return
end

# ╔═╡ 2215191c-73b6-456b-b36c-1ac3e22c63c9
#actions
function latency(RPM)
	latency_return = 60000 * RPM
	return latency_return
end

# ╔═╡ 5a375430-7a8e-43fd-b835-e6a8c5a0d2a8
#actions
function throughput(rate, time)
	throughput_return = rate * time
	return throughput_return
end

# ╔═╡ e00b5e57-a8a9-493e-8ed6-1eb1f1caef57
#actions
function success_rate(total_set, successful_set)
	success_rate_return = (total_set/successful_set) * 100
	return success_rate_return
end

# ╔═╡ bd50c8b2-d7c1-4b2d-8b7b-3e57ded5de02

		

# ╔═╡ 72ebc6db-4c3d-4650-b98e-0b5aa3a84beb
begin
	
	function selector(option_select_num)
		
		total_cost = 0
		cost = 1
		
		if option_select_num == 1
			
			#place in data
			"Enter outbound latency, processing_time and imbound latency in that order"
			#variables should take user input :preferable
			outbound_latency = 2
			processing_time =100
			imbound_latency = 200
			responce_time(outbound_latency, processing_time, imbound_latency)
			
		elseif option_select_num == 2
			
			"Enter number of RPM"
			#variables should take user input :preferable
			RPM = 6000
			latency(RPM)
			
		elseif option_select_num == 3
			
			"Enter rate and time in that order to calculate thoughput"
			#variables should take user input :preferable
			rate = 60
			time = 7000
			throughput(rate, time)
			
		else option_select_num == 4
			
			"Enter total set and successful sets in that order"
			#variables should take user input :preferable
			total_set = 100
			successful_set = 60
			success_rate(total_set, successful_set)

			
			
		end
	end
	#"Press 1: for responce time, 2: for latency, 3: for thoughput adn 4: success rate"
	
end
	

# ╔═╡ dcd176e9-3fcd-4ffe-866b-dcbd4d80b4b5
begin
	
		"Enter 1: for responce time, 2: for latency, 3: for thoughput or 4: success rate in selector function"
	option_select_num = 2 #user input for constraint selector
	selector(option_select_num)
end

# ╔═╡ 00000000-0000-0000-0000-000000000001
PLUTO_PROJECT_TOML_CONTENTS = """
[deps]
"""

# ╔═╡ 00000000-0000-0000-0000-000000000002
PLUTO_MANIFEST_TOML_CONTENTS = """
# This file is machine-generated - editing it directly is not advised

julia_version = "1.7.2"
manifest_format = "2.0"

[deps]
"""

# ╔═╡ Cell order:
# ╠═2b466abb-1e66-4d89-9565-f56b4f82e9d5
# ╠═39971b20-ca63-11ec-07e5-d14038fe071a
# ╠═2215191c-73b6-456b-b36c-1ac3e22c63c9
# ╠═5a375430-7a8e-43fd-b835-e6a8c5a0d2a8
# ╠═e00b5e57-a8a9-493e-8ed6-1eb1f1caef57
# ╠═bd50c8b2-d7c1-4b2d-8b7b-3e57ded5de02
# ╠═72ebc6db-4c3d-4650-b98e-0b5aa3a84beb
# ╠═dcd176e9-3fcd-4ffe-866b-dcbd4d80b4b5
# ╟─00000000-0000-0000-0000-000000000001
# ╟─00000000-0000-0000-0000-000000000002
